using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using FamilyHubs.ServiceDirectory.Core.Distance;
using FamilyHubs.ServiceDirectory.Core.Pagination;
using FamilyHubs.ServiceDirectory.Core.Pagination.Interfaces;
using FamilyHubs.ServiceDirectory.Core.Postcode.Interfaces;
using FamilyHubs.ServiceDirectory.Core.Postcode.Model;
using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Interfaces;
using FamilyHubs.ServiceDirectory.Web.Content;
using FamilyHubs.ServiceDirectory.Web.Filtering.Interfaces;
using FamilyHubs.ServiceDirectory.Web.Mappers;
using FamilyHubs.ServiceDirectory.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

namespace FamilyHubs.ServiceDirectory.Web.Pages.ServiceFilter;

public class ServiceFilterModel : PageModel
{
    public IEnumerable<IFilter> Filters { get; set; }
    //todo: into Filters (above)
    public IFilterSubGroups TypeOfSupportFilter { get; set; }
    public string? Postcode { get; set; }
    public string? AdminDistrict { get; set; }
    public float? Latitude { get; set; }
    public float? Longitude { get; set; }
    public IEnumerable<Service> Services { get; set; }
    public bool OnlyShowOneFamilyHubAndHighlightIt { get; set; }
    public bool FromPostcodeSearch { get; set; }
    public int CurrentPage { get; set; }
    public IPagination Pagination { get; set; }

    private readonly IServiceDirectoryClient _serviceDirectoryClient;
    private readonly IPostcodeLookup _postcodeLookup;
    private const int PageSize = 10;

    public ServiceFilterModel(IServiceDirectoryClient serviceDirectoryClient, IPostcodeLookup postcodeLookup)
    {
        _serviceDirectoryClient = serviceDirectoryClient;
        _postcodeLookup = postcodeLookup;
        Filters = FilterDefinitions.Filters;
        TypeOfSupportFilter = FilterDefinitions.CategoryFilter;
        Services = Enumerable.Empty<Service>();
        OnlyShowOneFamilyHubAndHighlightIt = false;
        Pagination = new DontShowPagination();
    }

    public Task<IActionResult> OnPost(string? postcode, string? adminDistrict)
    {
        CheckParameters(postcode);

        return HandlePost(postcode, adminDistrict);
    }

    //todo: input hidden for postcode etc. so don't keep getting
    //todo: test postcode error handling
    //todo: no need for the remaining 2 params
    private async Task<IActionResult> HandlePost(string postcode, string? adminDistrict)
    {
        dynamic routeValues;

        if (adminDistrict == null)
        {
            var (postcodeError, postcodeInfo) = await _postcodeLookup.Get(postcode);
            if (postcodeError != PostcodeError.None)
            {
                return RedirectToPage("/PostcodeSearch/Index", new { postcodeError });
            }

            routeValues = new
            {
                postcode = postcodeInfo!.Postcode,
                // todo: rename AdminDistrict
                adminDistrict = postcodeInfo.AdminArea,
                latitude = postcodeInfo.Latitude,
                longitude = postcodeInfo.Longitude,
                fromPostcodeSearch = true
            };
        }
        else
        {
            //todo: remove redundant remove handling in filters
            //todo: move into method
            //todo: remove all
            //todo: remove all filters of type when multiple
            //todo: remove all went back to postcode search (fromPostcodeSearch?)

            //todo: method to get key and value
            string? remove = Request.Form[IFilter.RemoveKey];
            string? removeKey = null, removeValue = null;
            if (remove != null)
            {
                if (remove == IFilter.RemoveAllValue)
                {
                    removeKey = IFilter.RemoveAllValue;
                }
                else
                {
                    int filterNameEndPos = remove.IndexOf("--", StringComparison.Ordinal);
                    //todo: think through if this is the right way to handle -1
                    if (filterNameEndPos != -1)
                    {
                        removeKey = remove[..filterNameEndPos];
                        removeValue = remove[(filterNameEndPos + 2)..];
                    }
                }
            }

            //todo: remove pageNum when remove != null, to go back to page 1 when removing a/all filter

            //todo: test removing -option-selected
            var filteredForm = Request.Form
                .Where(kvp => KeepParam(kvp.Key, removeKey));

            //todo: -option-selected handling
            //todo: children=all works, run through to check
            //todo: empty values are getting cleaned up, so no e.g. activities= . run through to check

            if (removeValue != null)
            {
                filteredForm = filteredForm.Select(kvp => RemoveFilterValue(kvp, removeKey!, removeValue));
            }

            routeValues = new ExpandoObject();
            var routeValuesDictionary = (IDictionary<string, object>)routeValues;

            foreach (var keyValuePair in filteredForm)
            {
                // ToString() stops RedirectToPage() using key=foo&key=bar, and instead we get key=foo,bar which we unpick on the GET
                routeValuesDictionary[keyValuePair.Key] = keyValuePair.Value.ToString();
            }
        }

        return RedirectToPage("/ServiceFilter/Index", routeValues);
    }

    private static KeyValuePair<string, StringValues> RemoveFilterValue(
        KeyValuePair<string, StringValues> kvp, string removeKey, string removeValue)
    {
        if (kvp.Key != removeKey)
            return kvp;

        var values = kvp.Value.ToList();
        values.Remove(removeValue);
        return new KeyValuePair<string, StringValues>(kvp.Key, new StringValues(values.ToArray()));
    }

    // simpler than asking all the filters to remove themselves
    private static HashSet<string> _parametersWhitelist = new()
    {
        "postcode",
        "admindistrict",
        "latitude",
        "longitude",
    };

    private static bool KeepParam(string key, string? removeKey)
    {
        if (key == "__RequestVerificationToken" || key.StartsWith(IFilter.RemoveKey))
            return false;

        if (removeKey == null)
            return true;

        //todo: move this logic out of here?
        if (removeKey == IFilter.RemoveAllValue)
        {
            return _parametersWhitelist.Contains(key.ToLowerInvariant());
        }

        // if we're removing a filter, go back to page 1
        if (key == QueryParamKeys.PageNum)
            return false;

        // key.StartsWith rather than '!= remove' to also remove [key]-option-selected
        //return !key.StartsWith(removeKey);

        // keeping, but might remove the only value still
        return true;
    }

    public Task<IActionResult> OnGet(string? postcode, string? adminDistrict, float? latitude, float? longitude, int? pageNum, bool? fromPostcodeSearch)
    {
        if (AnyParametersMissing(postcode, adminDistrict, latitude, longitude))
        {
            // handle cases:
            //todo: check
            // * when user goes filter page => cookie page => back link from success banner
            // * user manually removes query parameters from url
            // * user goes directly to page by typing it into the address bar
            return Task.FromResult<IActionResult>(RedirectToPage("/PostcodeSearch/Index"));
        }

        return HandleGet(postcode!, adminDistrict!, latitude!.Value, longitude!.Value, pageNum, fromPostcodeSearch);
    }

    private static bool AnyParametersMissing(string? postcode, string? adminDistrict, float? latitude, float? longitude)
    {
        return string.IsNullOrEmpty(postcode)
               || string.IsNullOrEmpty(adminDistrict)
               || latitude == null
               || longitude == null;
    }

    private async Task<IActionResult> HandleGet(string postcode, string adminDistrict, float latitude, float longitude, int? pageNum, bool? fromPostcodeSearch)
    {
        FromPostcodeSearch = fromPostcodeSearch == true;
        Postcode = postcode;
        AdminDistrict = adminDistrict;
        Latitude = latitude;
        Longitude = longitude;
        CurrentPage = pageNum ?? 1;

        //todo: display friendly ids in url?

        // if we've just come from the postcode search, go with the configured default filter options
        // otherwise, apply the filters from the query parameters
        if (!FromPostcodeSearch)
        {
            Filters = FilterDefinitions.Filters.Select(fd => fd.ToPostFilter(Request.Query));
            TypeOfSupportFilter = FilterDefinitions.CategoryFilter.ToPostFilter(Request.Query);
        }

        (Services, Pagination) = await GetServicesAndPagination(adminDistrict, latitude, longitude);

        return Page();
    }

    private static void CheckParameters([NotNull] string? postcode)
    {
        ArgumentException.ThrowIfNullOrEmpty(postcode);
    }

    private async Task<(IEnumerable<Service>, IPagination)> GetServicesAndPagination(string adminDistrict, float latitude, float longitude)
    {
        //todo: add method to filter to add its filter criteria to a request object sent to getservices.., then call in a foreach loop
        // or references to the individual filters too, so we don't keep iterating them
        //todo: work with selected aspects, rather than values
        // have value as a property of the aspect, and use that in the razor
        int? searchWithinMeters = null;
        var searchWithinFilter = Filters.First(f => f.Name == FilterDefinitions.SearchWithinFilterName);
        var searchWithinFilterAspect = searchWithinFilter.SelectedAspects.FirstOrDefault();
        if (searchWithinFilterAspect != null)
        {
            searchWithinMeters = DistanceConverter.MilesToMeters(int.Parse(searchWithinFilterAspect.Id));
        }

        bool? isPaidFor = null;
        var costFilter = Filters.First(f => f.Name == FilterDefinitions.CostFilterName);
        if (costFilter.SelectedAspects.Count() == 1)
        {
            isPaidFor = costFilter.SelectedAspects.First().Id == "pay-to-use";
        }

        bool? familyHubFilter = null;
        var showFilter = Filters.First(f => f.Name == FilterDefinitions.ShowFilterName);
        switch (showFilter.SelectedAspects.Count())
        {
            case 0:
                OnlyShowOneFamilyHubAndHighlightIt = true;
                break;
            case 1:
                familyHubFilter = bool.Parse(showFilter.SelectedAspects.First().Id);
                break;
            //case 2: there are only 2 options, so if both are selected, there's no need to filter
        }

        int? givenAge = null;
        var childrenFilter = Filters.First(f => f.Name == FilterDefinitions.ChildrenAndYoungPeopleFilterName);
        var childFilterAspect = childrenFilter.SelectedAspects.FirstOrDefault();
        if (childFilterAspect != null && childFilterAspect.Id != FilterDefinitions.ChildrenAndYoungPeopleAllId)
        {
            givenAge = int.Parse(childFilterAspect.Id);
        }

        var taxonomyIds = TypeOfSupportFilter.SelectedAspects.Select(a => a.Id);

        var services = await _serviceDirectoryClient.GetServicesWithOrganisation(
            adminDistrict,
            latitude,
            longitude,
            searchWithinMeters,
            givenAge,
            isPaidFor,
            OnlyShowOneFamilyHubAndHighlightIt ? 1 : null,
            familyHubFilter,
            taxonomyIds,
            CurrentPage,
            PageSize);

        var pagination = new LargeSetPagination(services.TotalPages, CurrentPage);

        return (ServiceMapper.ToViewModel(services.Items), pagination);
    }
}
