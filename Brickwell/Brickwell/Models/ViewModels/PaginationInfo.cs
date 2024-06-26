﻿namespace Brickwell.Models.ViewModels
{
    public class PaginationInfo
    {
        public int CurrentPage { get; set; }
        public int ItemsPerPage { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int) (Math.Ceiling((decimal)TotalItems / ItemsPerPage));

        public int MaxVisiblePages = 5;
    }
}
