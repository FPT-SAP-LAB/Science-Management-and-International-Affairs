using ENTITIES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL.InternationalCollaboration.AcademicActivity;
using System.Data.SqlClient;
using System.Data.Entity;

namespace BLL.InternationalCollaboration.AcademicActivity
{
    public class FormRepo
    {
        ScienceAndInternationalAffairsEntities db = new ScienceAndInternationalAffairsEntities();
        public DetailOfAcademicActivityRepo.baseForm getFormbyPhase(int phase_id)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                Form f = db.Forms.Where(x => x.phase_id == phase_id).FirstOrDefault();
                string sql = @"SELECT q.question_id, q.title, at.answer_type_id,cast(q.is_compulsory as int) as is_compulsory,cast(q.is_changeable as int) as is_changeable 
                                    FROM SMIA_AcademicActivity.Form f
                                    INNER JOIN SMIA_AcademicActivity.Question q ON q.form_id = f.form_id
                                    INNER JOIN SMIA_AcademicActivity.AnswerType at ON at.answer_type_id = q.answer_type_id
                                    where f.phase_id = @phase_id order by q.is_changeable";
                List<DetailOfAcademicActivityRepo.Ques> ques = db.Database.SqlQuery<DetailOfAcademicActivityRepo.Ques>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                string ques_id = "";
                List<int> type = new List<int> { 3, 5 };
                foreach (DetailOfAcademicActivityRepo.Ques q in ques)
                {
                    if (type.Contains(q.answer_type_id))
                    {
                        ques_id += q.question_id + ",";
                    }
                }
                List<DetailOfAcademicActivityRepo.QuesOption> quesOption = new List<DetailOfAcademicActivityRepo.QuesOption>();
                if (!String.IsNullOrEmpty(ques_id))
                {
                    ques_id = ques_id.Remove(ques_id.Length - 1);
                    sql = @"select qo.* from SMIA_AcademicActivity.QuestionOption qo where qo.question_id in (" + ques_id + ")";
                    quesOption = db.Database.SqlQuery<DetailOfAcademicActivityRepo.QuesOption>(sql).ToList();
                }
                DetailOfAcademicActivityRepo.baseForm data = new DetailOfAcademicActivityRepo.baseForm
                {
                    form = f,
                    ques = ques,
                    quesOption = quesOption
                };
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return new DetailOfAcademicActivityRepo.baseForm();
            }
        }
        public bool updateForm(DetailOfAcademicActivityRepo.baseForm data, List<DetailOfAcademicActivityRepo.CustomQuestion> data_unchange)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    List<Question> questions = db.Questions.Where(x => x.form_id == data.form.form_id).ToList();
                    List<int> quess_id = questions.Select(x => x.question_id).ToList();
                    Form f = db.Forms.Find(data.form.form_id);
                    if (f == null)
                    {
                        f = db.Forms.Add(new Form
                        {
                            title = data.form.title == null ? String.Empty : data.form.title,
                            title_description = data.form.title_description == null ? String.Empty : data.form.title_description,
                            phase_id = data.form.phase_id
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        f.title = data.form.title == null ? String.Empty : data.form.title;
                        f.title_description = data.form.title_description == null ? String.Empty : data.form.title_description;
                        db.Entry(f).State = EntityState.Modified;
                    }
                    updateQuestion(data, f, quess_id);
                    foreach (DetailOfAcademicActivityRepo.CustomQuestion cq in data_unchange)
                    {
                        db.Questions.Add(new Question
                        {
                            form_id = f.form_id,
                            title = cq.title == null ? String.Empty : cq.title,
                            answer_type_id = 1,
                            is_compulsory = cq.is_compulsory == 1 ? true : false,
                            is_changeable = false
                        });
                    }
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public void updateQuestion(DetailOfAcademicActivityRepo.baseForm data, Form f, List<int> quess_id)
        {
            foreach (DetailOfAcademicActivityRepo.Ques q in data.ques)
            {
                if (quess_id.Contains(q.question_id))
                {
                    updateQuesOption(q, data);
                }
                else
                {
                    addQuesOption(q, data, f);
                }
                quess_id.Remove(q.question_id);
            }
            removeQues(quess_id);
        }
        public void updateQuesOption(DetailOfAcademicActivityRepo.Ques q, DetailOfAcademicActivityRepo.baseForm data)
        {
            Question qt = db.Questions.Find(q.question_id);
            qt.title = q.title == null ? String.Empty : q.title;
            qt.answer_type_id = q.answer_type_id;
            qt.is_compulsory = q.is_compulsory == 1 ? true : false;
            qt.is_changeable = true;
            db.Entry(qt).State = EntityState.Modified;
            if (q.answer_type_id == 3 || q.answer_type_id == 5)
            {
                QuestionOption qo = db.QuestionOptions.Where(x => x.question_id == q.question_id).FirstOrDefault();
                if (data.quesOption != null)
                {
                    DetailOfAcademicActivityRepo.QuesOption qon = data.quesOption.Find(x => x.question_id == q.question_id);
                    if (qo != null)
                    {
                        if (qon != null)
                        {
                            qo.option_title = qon.option_title == null ? String.Empty : qon.option_title;
                            db.Entry(qo).State = EntityState.Modified;
                        }
                        else
                        {
                            db.QuestionOptions.Remove(qo);
                        }
                    }
                    else
                    {
                        if (qon != null)
                        {
                            db.QuestionOptions.Add(new QuestionOption
                            {
                                question_id = q.question_id,
                                option_title = qon.option_title
                            });
                        }
                    }
                    db.SaveChanges();
                }
            }
        }
        public void addQuesOption(DetailOfAcademicActivityRepo.Ques q, DetailOfAcademicActivityRepo.baseForm data, Form f)
        {
            Question qn = db.Questions.Add(new Question
            {
                form_id = f.form_id,
                answer_type_id = q.answer_type_id,
                title = q.title == null ? String.Empty : q.title,
                is_compulsory = q.is_compulsory == 1 ? true : false,
                is_changeable = true
            });
            db.SaveChanges();
            if (q.answer_type_id == 3 || q.answer_type_id == 5)
            {
                if (data.quesOption != null)
                {
                    DetailOfAcademicActivityRepo.QuesOption qon = data.quesOption.Find(x => x.question_id == q.question_id);
                    if (qon != null)
                    {
                        db.QuestionOptions.Add(new QuestionOption
                        {
                            question_id = qn.question_id,
                            option_title = qon.option_title
                        });
                    }
                }
            }
        }
        public void removeQues(List<int> quess_id)
        {
            foreach (int i in quess_id)
            {
                Question q = db.Questions.Find(i);
                if (q.answer_type_id == 3 || q.answer_type_id == 5)
                {
                    QuestionOption qo = db.QuestionOptions.Where(x => x.question_id == i).FirstOrDefault();
                    if (qo != null)
                    {
                        db.QuestionOptions.Remove(qo);
                    }
                }
                db.Questions.Remove(q);
            }
        }
        public bool deleteForm(int phase_id)
        {
            using (DbContextTransaction transaction = db.Database.BeginTransaction())
            {
                try
                {
                    Form f = db.Forms.Where(x => x.phase_id == phase_id).FirstOrDefault();
                    db.Forms.Remove(f);
                    db.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    transaction.Rollback();
                    return false;
                }
            }
        }
        public viewResponse getResponse(int phase_id)
        {
            try
            {
                string sql = @"select q.* from SMIA_AcademicActivity.Question q
                                inner join SMIA_AcademicActivity.Form f on q.form_id = f.form_id
                                where f.phase_id = @phase_id order by q.is_changeable";
                List<Question> ques = db.Database.SqlQuery<Question>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                sql = @"select r.answer from SMIA_AcademicActivity.Form f
                            inner join SMIA_AcademicActivity.Response r on f.form_id = r.form_id
                            where f.phase_id = @phase_id";
                List<Answer> res = db.Database.SqlQuery<Answer>(sql, new SqlParameter("phase_id", phase_id)).ToList();
                viewResponse data = new viewResponse
                {
                    ques = ques,
                    res = res
                };
                return data;
            }
            catch (Exception e)
            {
                return new viewResponse();
            }
        }
        public class viewResponse
        {
            public List<Question> ques { get; set; }
            public List<Answer> res { get; set; }
        }
        public class Answer
        {
            public string answer { get; set; }
        }
    }
}
