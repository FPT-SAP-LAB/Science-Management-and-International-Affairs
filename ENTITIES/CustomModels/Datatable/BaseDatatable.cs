﻿using System;
using System.Web;

namespace ENTITIES.CustomModels.Datatable
{
    public class BaseDatatable
    {
        public int Start { get; set; }
        public int Length { get; set; }
        public string SortColumnName { get; set; }
        public string SortDirection { get; set; }
        public BaseDatatable() { }
        public BaseDatatable(HttpRequestBase request)
        {
            Start = Convert.ToInt32(request["start"]);
            Length = Convert.ToInt32(request["length"]);
            SortColumnName = request["columns[" + request["order[0][column]"] + "][name]"];
            SortDirection = request["order[0][dir]"];
        }
    }
}
