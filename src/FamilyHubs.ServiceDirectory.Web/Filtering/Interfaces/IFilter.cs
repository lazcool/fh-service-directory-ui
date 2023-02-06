﻿namespace FamilyHubs.ServiceDirectory.Web.Filtering.Interfaces;

public interface IFilter
{
    public const string RemoveKey = "remove";
    public const string RemoveAllValue = "all";

    string Name { get; }
    string Description { get; }
    FilterType FilterType { get; }
    IEnumerable<IFilterAspect> Aspects { get; }
    IEnumerable<IFilterAspect> SelectedAspects { get; }
    bool IsSelected(IFilterAspect aspect);
    IFilter ToPostFilter(IQueryCollection query);
}