﻿using ENTITIES;

namespace BLL.ModelDAL
{
    public class BaseRequestRepo
    {
        ScienceAndInternationalAffairsEntities db;
        public bool IsEnded(int request_id)
        {
            db = new ScienceAndInternationalAffairsEntities();
            return db.BaseRequests.Find(request_id).finished_date != null;
        }
    }
}
