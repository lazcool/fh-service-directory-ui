using FamilyHubs.ServiceDirectory.Core.ServiceDirectory.Interfaces;
using FamilyHubs.ServiceDirectory.Shared.Models.Api.OpenReferralServices;
using FamilyHubs.ServiceDirectory.Web.Models;
using FamilyHubs.ServiceDirectory.Web.Models.Interfaces;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics;

namespace FamilyHubs.ServiceDirectory.Web.Pages
{
    public class ServiceFilterModel : PageModel
    {
        private readonly IServiceDirectoryClient _serviceDirectoryClient;

        //todo: partial?
        public static readonly FilterSubGroups CategoryFilter = new("category", "Category", new Filter[]
        {
            //todo: can we get these from the db?
            new("activities", "Activities, clubs and groups", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("activities--activities", "Activities"),
                new FilterAspect("activities--school-clubs", "Before and after school clubs"),
                new FilterAspect("activities--holiday-clubs", "Holiday clubs and schemes"),
                new FilterAspect("activities--music-arts-dance", "Music, arts and dance"),
                new FilterAspect("activities--parent-group", "Parent, baby and toddler groups"),
                new FilterAspect("activities--preschool-playgroup", "Pre-school playgroup"),
                new FilterAspect("activities--sports", "Sports and recreation")
            }),
            new("family-support", "Family support", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("family-support--bullying", "Bullying and cyber bullying"),
                new FilterAspect("family-support--debt-advice", "Debt and welfare advice"),
                new FilterAspect("family-support--domestic-abuse", "Domestic abuse"),
                new FilterAspect("family-support--intensive", "Intensive targeted family support"),
                new FilterAspect("family-support--money-benefits-housing", "Money, benefits and housing"),
                new FilterAspect("family-support--parenting", "Parenting support"),
                new FilterAspect("family-support--reducing-parental-conflict", "Reducing parental conflict"),
                new FilterAspect("family-support--separation-support", "Separating and separated parent support"),
                new FilterAspect("family-support--stopping-smoking", "Stopping smoking"),
                new FilterAspect("family-support--substance-misuse", "Substance misuse (including alcohol and drug)"),
                new FilterAspect("family-support--targeted-youth", "Targeted youth support"),
                new FilterAspect("family-support--youth-justice", "Youth justice services")
            }),
            new("health", "Health", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("health--hearing-sight", "Hearing and sight"),
                new FilterAspect("health--nutrition", "Nutrition and weight management"),
                new FilterAspect("health--oral", "Oral health"),
                new FilterAspect("health--public", "Public health services"),
                new FilterAspect("health--mental", "Mental health, social and emotional support")
            }),
            new("pregnancy", "Pregnancy, birth and early years", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("pregnancy--early-years", "Early years language and learning"),
                new FilterAspect("pregnancy--birth-registration", "Birth registration"),
                new FilterAspect("pregnancy--infant-feeding", "Infant feeding support (including breastfeeding)"),
                new FilterAspect("pregnancy--midwife", "Midwife and maternity"),
                new FilterAspect("pregnancy--perinatal_mental",
                    "Perinatal mental health support (pregnancy to one year post birth)"),
                new FilterAspect("pregnancy--health-visiting", "Health visiting")
            }),
            new("send", "Special educational needs and disabilities (SEND) support", FilterType.Checkboxes,
                new IFilterAspect[]
                {
                    new FilterAspect("send--early-years", "Early years support"),
                    new FilterAspect("send--asd", "Autistic Spectrum Disorder (ASD)"),
                    new FilterAspect("send--breaks", "Breaks and respite"),
                    new FilterAspect("send--parents-carers", "Groups for parents and carers of children with SEND"),
                    new FilterAspect("send--hearing-impairment", "Hearing impairment"),
                    new FilterAspect("send--multi-sensory-impairment", "Multi-sensory impairment"),
                    new FilterAspect("send--physical-disabilities", "Physical disabilities"),
                    new FilterAspect("send--learning-difficulties", "Learning difficulties and disabilities"),
                    new FilterAspect("send--social-support", "Social, emotional and mental health support"),
                    new FilterAspect("send--speech", "Speech, language and communication needs"),
                    new FilterAspect("send--visual-impairment", "Visual impairment"),
                    new FilterAspect("send--other-difficulties", "Other difficulties or disabilities")
                }),
            new("transport", "Transport", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("transport--community", "Community transport")
            })
        });

        public static readonly IEnumerable<Filter> Filters = new[]
        {
            new Filter("cost", "Cost", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("cost--free", "Free"),
                new FilterAspect("cost--pay-to-use", "Pay to use")
            }),
            new Filter("show", "Show", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("show--family-hubs", "Family hubs"),
                new FilterAspect("show--services-and-groups", "Services and groups")
            }),
            new Filter("search-within", "Search within", FilterType.Radios, new IFilterAspect[]
            {
                new FilterAspect("search-within--1-mile", "1 mile"),
                new FilterAspect("search-within--2-miles", "2 miles"),
                new FilterAspect("search-within--5-miles", "5 miles"),
                new FilterAspect("search-within--10-miles", "10 miles"),
                new FilterAspect("search-within--20-miles", "20 miles")
            }),
            new Filter("age-range", "Age range", FilterType.Checkboxes, new IFilterAspect[]
            {
                new FilterAspect("age-range--all-age-groups", "All age groups"),
                new FilterAspect("age-range--0-to-5", "0 to 5"),
                new FilterAspect("age-range--6-to-11", "6 to 11"),
                new FilterAspect("age-range--12-to-15", "12 to 15"),
                new FilterAspect("age-range--16-to-18", "16 to 18"),
                new FilterAspect("age-range--19-to-25-with-send", "19 to 25 with SEND"),
                new FilterAspect("age-range--parents-and-carers", "Parents and carers")
            })
        };

        public string? Postcode { get; set; }
        public IEnumerable<Service> Services { get; set; }
        public bool OnlyShowOneFamilyHubAndHighlightIt { get; set; }

        public ServiceFilterModel(IServiceDirectoryClient serviceDirectoryClient)
        {
            _serviceDirectoryClient = serviceDirectoryClient;
            Services = Enumerable.Empty<Service>();
            // we set this to true when neither show filter is selected
            OnlyShowOneFamilyHubAndHighlightIt = true;
        }

        public async Task OnGet(string? postcode, string? adminDistrict, float? latitude, float? longitude)
        {
            Postcode = postcode;

            if (adminDistrict == null)
            {
                //todo:
                throw new NotImplementedException();
            }

            // example postcode for salford la : m27 8ss

            //todo: missing params
            var services = await _serviceDirectoryClient.GetServices(
                adminDistrict,
                latitude!.Value,
                longitude!.Value);
            Services = ToServiceViewModel(services.Items);
        }

        //todo: where live?
        private IEnumerable<Service> ToServiceViewModel(IEnumerable<OpenReferralServiceDto> serviceDto)
        {
            // open questions
            // --------------
            // do we display 'run by'? if so, where do we get it from? (line in description?)
            // is valid from/valid to going to be populated? should we filter by it?
            // assumptions
            // -----------
            // General:
            // if data missing for a field, we don't show the row at all (as opposed to displaying the key with a blank value)
            // Address:
            // show first location's first address only
            // omit empty address lines
            // Phone:
            // show first phone number of first contact, if there is one (ignore >1 contact or contact with >1 number)
            // Website:
            // there is no description field for the link, so have used the name of the service (prototype shows unique website names, e.g. CAMHS for Child and Adolescent Mental Health Services (CAMHS)
            // Cost:
            // if no cost options present, we assume is 'Free'
            // if more than one cost, we display them all on separate lines (only single cost shown on the prototype)
            // construct as �{amount} every {amount_description} - assumes format will always work, amount in pounds
            // we ignore valid from/valid to

            return serviceDto.Select(ToServiceViewModel);
        }

        private Service ToServiceViewModel(OpenReferralServiceDto dto)
        {
            //todo: move into helper
            Debug.Assert(dto.ServiceType.Name == "Family Experience");

            //todo: check got one. always the first??
            var serviceAtLocation = dto.Service_at_locations?.FirstOrDefault();
            var address = serviceAtLocation?.Location.Physical_addresses?.FirstOrDefault();
            var eligibility = dto.Eligibilities?.FirstOrDefault();

            // or check id == d242700a-b2ad-42fe-8848-61534002156c instead??
            //todo: just double check null Taxonomy
            bool isFamilyHub = dto.Service_taxonomys?.Any(t => t.Taxonomy?.Name == "FamilyHub") ?? false;

            IEnumerable<string> cost;
            if (dto.Cost_options?.Any() == false)
            {
                cost = new[] {"Free"};
            }
            else
            {
                cost = dto.Cost_options!.Select(co => $"�{co.Amount} every {co.Amount_description}");
            }

            return new Service(
                isFamilyHub ? ServiceType.FamilyHub : ServiceType.Service,
                dto.Name,
                //todo: tidy
                (dto.Distance / 1609.34),
                cost,
                //todo: do we capture this? where?
                null,
                //todo: tidy up. what about SEND??
                $"{eligibility?.Minimum_age} to {eligibility?.Maximum_age}",
                //todo: Regular_schedule off service or serviceatlocation?
                serviceAtLocation?.Regular_schedule?.FirstOrDefault()?.Description,
                RemoveEmpty(address?.Address_1, address?.City, address?.State_province, address?.Postal_code),
                dto.Contacts?.FirstOrDefault()?.Phones?.FirstOrDefault()?.Number,
                dto.Email,
                dto.Name,
                dto.Url);
        }

        private static IEnumerable<string> RemoveEmpty(params string?[] list)
        {
            return list.Where(x => !string.IsNullOrWhiteSpace(x))!;
        }
    }
}