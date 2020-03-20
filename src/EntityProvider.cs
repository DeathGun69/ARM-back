using System;
using System.Collections;
using System.Collections.Generic;

namespace TeacherARMBackend {
    public class GroupHelper : DataBaseSqlHelper<Group>
    {    
        public override string TableName => "arm_group";

        public GroupHelper(DataBaseAccessor accessor) : base(accessor) {}
    }
    public class CompetenceHelper : DataBaseSqlHelper<Competence> {
        public override string TableName => "competence";
        public CompetenceHelper(DataBaseAccessor accessor) : base(accessor) {}
    }
    public class CourseHelper : DataBaseSqlHelper<Course> {
        public override string TableName => "course";
        public CourseHelper(DataBaseAccessor accessor) : base(accessor) {}
    } 
    public class SectionHelper : DataBaseSqlHelper<Section> {
        public override string TableName => "ssection";
        public SectionHelper(DataBaseAccessor accessor) : base(accessor) {}
    } 
   
    public class ThemeHelper : DataBaseSqlHelper<Theme> {
        public override string TableName => "theme";
        public ThemeHelper(DataBaseAccessor accessor) : base(accessor) {}
    } 


    public class UserHelper : DataBaseSqlHelper<User> {
        public override string TableName => "arm_user";
        public UserHelper(DataBaseAccessor accessor) : base(accessor) {}
    }    

    public class LessonHelper : DataBaseSqlHelper<Lesson> {
        public override string TableName => "lesson";
        public LessonHelper(DataBaseAccessor accessor) : base(accessor) {}
    }

    public class ThemeCompetenceHelper : DataBaseSqlHelper<ThemeCompetence> {
        public override string TableName => "theme_competence";
        public ThemeCompetenceHelper(DataBaseAccessor accessor) : base(accessor) {}
    }

    public class CoursePlanHelper :DataBaseSqlHelper<CoursePlan> {
        public override string TableName => "course_plan";
        public CoursePlanHelper(DataBaseAccessor accessor) : base(accessor) {}
    }

    public class CourseGroupsHelper :DataBaseSqlHelper<CourseGroups> {
        public override string TableName => "course_groups";
        public CourseGroupsHelper(DataBaseAccessor accessor) : base(accessor) {}
    }
    
    public class EntityProvider {
        public static EntityProvider Instance {get; } = new EntityProvider();        
        private DataBaseAccessor _accessor = new DataBaseAccessor();
        public Dictionary<string, dynamic> Tables {get; }
        public UserHelper UserHelper {get; }
        public ThemeHelper ThemeHelper {get; }
        public CourseHelper CourseHelper {get; }
        public SectionHelper SectionHelper {get; }
        public CoursePlanHelper CoursePlanHelper {get; }
        
        public GroupHelper GroupHelper {get; }
        public CourseGroupsHelper CourseGroupsHelper {get; }
        
        public CompetenceHelper CompetenceHelper {get; }
        public LessonHelper LessonHelper {get; }
        public ThemeCompetenceHelper ThemeCompetenceHelper {get;}
        

        private EntityProvider() {
            UserHelper = new UserHelper(_accessor);
            ThemeHelper = new ThemeHelper(_accessor);
            SectionHelper = new SectionHelper(_accessor);
            CourseHelper = new CourseHelper(_accessor);
            GroupHelper = new GroupHelper(_accessor);
            CompetenceHelper = new CompetenceHelper(_accessor);

            CourseGroupsHelper = new CourseGroupsHelper(_accessor);
            CoursePlanHelper = new CoursePlanHelper(_accessor);
            LessonHelper = new LessonHelper(_accessor);
            ThemeCompetenceHelper = new ThemeCompetenceHelper(_accessor);

            Tables = new Dictionary<string, dynamic>() {
                { "course", CourseHelper },
                { "section" , SectionHelper},
                { "user" ,  UserHelper},
                { "competence" , CompetenceHelper },
                { "theme" , ThemeHelper},
                { "group" , GroupHelper}, 
                { "course_groups", CourseGroupsHelper},
                { "course_plan", CoursePlanHelper},
                { "lesson", LessonHelper},
                { "theme_competence", ThemeCompetenceHelper}
            };
        }

    }
}