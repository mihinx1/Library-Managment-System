using Core.Library.Base;
using Csla;
using Csla.Core;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace IPSTemplate.UI.Blazor.Helpers
{
    public static class DataSourceRequestExtensions
    {
        public static async Task<MobileCslaRequest> ConvertToCslaRequestAsync(
            this DataSourceRequest @this,
            IDataPortal<MobileCslaRequest> mobileRequestPortal,
            IDataPortal<CslaRequestSort> requestSortPortal,
            IDataPortal<CslaRequestFilter> requestFilterPortal)
        {
            MobileCslaRequest gridRequest = await mobileRequestPortal.CreateAsync();

            if (@this is null)
                return gridRequest;

            // Paging
            gridRequest.Page = @this.Page;
            gridRequest.PageSize = @this.PageSize;

            // Sorts
            if (@this.Sorts != null && @this.Sorts.Count > 0)
                gridRequest.Sorts = @this.Sorts.ParseSortExpressionCollection(gridRequest.Sorts, requestSortPortal);

            // Filters
            if (@this.Filters != null && @this.Filters.Count > 0)
                gridRequest.Filters = @this.Filters.ParseFilterExpressionCollection(gridRequest.Filters, requestFilterPortal);

            return gridRequest;
        }

        public static MobileCslaRequest ConvertToCslaRequest(
            this DataSourceRequest @this,
            IDataPortal<MobileCslaRequest> mobileRequestPortal,
            IDataPortal<CslaRequestSort> requestSortPortal,
            IDataPortal<CslaRequestFilter> requestFilterPortal)
        {
            MobileCslaRequest gridRequest = mobileRequestPortal.Create();

            if (@this is null)
                return gridRequest;

            // Paging
            gridRequest.Page = @this.Page;
            gridRequest.PageSize = @this.PageSize;

            // Sorts
            if (@this.Sorts != null && @this.Sorts.Count > 0)
                gridRequest.Sorts = @this.Sorts.ParseSortExpressionCollection(gridRequest.Sorts, requestSortPortal);

            // Filters
            if (@this.Filters != null && @this.Filters.Count > 0)
                gridRequest.Filters = @this.Filters.ParseFilterExpressionCollection(gridRequest.Filters, requestFilterPortal);

            return gridRequest;
        }

        private static MobileList<CslaRequestSort> ParseSortExpressionCollection(
            this IList<SortDescriptor> collection,
            MobileList<CslaRequestSort> list,
            IDataPortal<CslaRequestSort> requestSortPortal)
        {
            foreach (var sort in collection)
            {
                list.Add(CslaRequestSort.CreateNewSort(
                    sort.Member,
                    sort.SortDirection == 0
                        ? System.ComponentModel.ListSortDirection.Ascending
                        : System.ComponentModel.ListSortDirection.Descending,
                    requestSortPortal));
            }

            return list;
        }

        private static MobileList<CslaRequestFilter> ParseFilterExpressionCollection(
            this IList<IFilterDescriptor> collection,
            MobileList<CslaRequestFilter> list,
            IDataPortal<CslaRequestFilter> requestFilterPortal)
        {
            foreach (var filterDescriptor in collection)
            {
                list = filterDescriptor.ParseFilterExpression(list, requestFilterPortal);
            }

            return list;
        }

        private static MobileList<CslaRequestFilter> ParseFilterExpression(
            this IFilterDescriptor filter,
            MobileList<CslaRequestFilter> list,
            IDataPortal<CslaRequestFilter> requestFilterPortal)
        {
            if (filter is FilterDescriptor filterDescriptor)
            {
                if (filterDescriptor.Value is null && ((int)filterDescriptor.Operator) <= 10)
                    return list;

                list.Add(CslaRequestFilter.CreateNewFilter(
                    filterDescriptor.Member,
                    filterDescriptor.MemberType,
                    (int)filterDescriptor.Operator,
                    filterDescriptor.Value,
                    requestFilterPortal));
            }
            else if (filter is CompositeFilterDescriptor compositeFilterDescriptor)
            {
                list = compositeFilterDescriptor.FilterDescriptors.ParseFilterExpressionCollection(list, requestFilterPortal);
            }

            return list;
        }

        public static string GetSearchString(this DropDownListReadEventArgs args)
        {
            if (args.Request.Filters.Count == 0)
                return "";

            var filterDescriptor = (FilterDescriptor)args.Request.Filters.First();
            return (string)filterDescriptor.Value;
        }
    }
}
