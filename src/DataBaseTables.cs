namespace TeacherARMBackend {


    public class Group : DataBaseEntity
    {        
        public string name { get; set; }
        public int code { get; set; }

    }
    public class User : DataBaseEntity
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string patronymic { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public bool is_admin {get; set;}
    }

    public class Competence : DataBaseEntity
    {
        public string name { get; set; }
        public string code { get; set; }
    }

    public class Course : DataBaseEntity
    {
        public string name { get; set; }
        public string univer { get; set; }
        public int hours { get; set; }
        public int id_teacher { get; set; }
        public string info {get; set;}

    }

    public class CourseGroups : DataBaseEntity 
    {
        public int id_group {get; set;}
        public int id_course {get; set; }
    }

    public class CoursePlan : DataBaseEntity {
        public int id_course {get; set;}
        public int id_section {get; set; }
    }

    public class Lesson : DataBaseEntity {
        public int hours {get; set;}
        public string type {get; set;}
        public int id_theme {get; set;}
    }
     public class Section : DataBaseEntity
    {
        public string name { get; set; }
    }


    public class Theme : DataBaseEntity
    {
        public string name { get; set; }        
        public int id_section { get; set; }   
        public string info {get; set;}
    }

    public class ThemeCompetence : DataBaseEntity {
        public int id_theme {get; set;}
        public int id_competence {get; set;}
    }

}