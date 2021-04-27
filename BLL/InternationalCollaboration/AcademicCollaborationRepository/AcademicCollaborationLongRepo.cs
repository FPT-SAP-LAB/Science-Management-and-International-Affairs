using ENTITIES;
using ENTITIES.CustomModels;
using System;
using System.Data.SqlClient;
using System.Linq;

namespace BLL.InternationalCollaboration.AcademicCollaborationRepository
{
    public class AcademicCollaborationLongRepo
    {
        private readonly ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        //LONG-TERM GET CONTENT
        public AlertModal<AcademicCollaborationTypeLanguage> GetLTContent(int collab_type_id, int language_id)
        {
            try
            {
                db.Configuration.LazyLoadingEnabled = false;
                AcademicCollaborationTypeLanguage ltContent = db.AcademicCollaborationTypeLanguages.Where(x => x.collab_type_id == collab_type_id && x.language_id == language_id).FirstOrDefault();
                return new AlertModal<AcademicCollaborationTypeLanguage>(ltContent, true, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<AcademicCollaborationTypeLanguage>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM UPDATE CONTENT
        public AlertModal<string> UpdateLTContent(int collab_type_id, int language_id, string description)
        {
            try
            {
                AcademicCollaborationTypeLanguage academicCollaborationTypeLanguage = db.AcademicCollaborationTypeLanguages.Find(language_id, collab_type_id);
                academicCollaborationTypeLanguage.description = description;
                db.SaveChanges();
                return new AlertModal<string>(null, true, "Thành công", "Cập nhật nội dung thành công.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM GET GOING || COMING CONTENT
        public AlertModal<CollaborationTypeDirectionLanguage> GetLTGCContent(int direction_id, int collab_type_id, int language_id)
        {
            try
            {
                var sql = @"select ctdl.*
                            from IA_AcademicCollaboration.CollaborationTypeDirection ctd
                            join IA_AcademicCollaboration.CollaborationTypeDirectionLanguage ctdl
                            on ctd.collab_type_direction_id = ctdl.collab_type_direction_id
                            where ctd.direction_id = @direction_id and ctd.collab_type_id = @collab_type_id
                            and ctdl.language_id = @language_id";
                CollaborationTypeDirectionLanguage ltgcContent = db.Database.SqlQuery<CollaborationTypeDirectionLanguage>(sql,
                    new SqlParameter("direction_id", direction_id),
                    new SqlParameter("collab_type_id", collab_type_id),
                    new SqlParameter("language_id", language_id)).FirstOrDefault();
                return new AlertModal<CollaborationTypeDirectionLanguage>(ltgcContent, true, null, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<CollaborationTypeDirectionLanguage>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }

        //LONG-TERM UPDATE GOING || COMING CONTENT
        public AlertModal<string> UpdateLTGCContent(int collab_type_direction_id, int language_id, string description)
        {
            try
            {
                CollaborationTypeDirectionLanguage collaborationTypeDirectionLanguage = db.CollaborationTypeDirectionLanguages.Find(collab_type_direction_id, language_id);
                collaborationTypeDirectionLanguage.description = description;
                db.SaveChanges();
                return new AlertModal<string>(null, true, "Thành công", "Cập nhật nội dung thành công.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new AlertModal<string>(null, false, "Lỗi", "Có lỗi xảy ra");
            }
        }
    }
}
