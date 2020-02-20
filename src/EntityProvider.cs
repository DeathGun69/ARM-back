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
        public override string TableName => "section";
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
    
    public class EntityProvider {
        public static EntityProvider Instance {get; } = new EntityProvider();        
        private DataBaseAccessor _accessor = new DataBaseAccessor();
        public Dictionary<string, dynamic> Tables {get; }
        public UserHelper UserHelper {get; }
        public ThemeHelper ThemeHelper {get; }
        public SectionHelper SectionHelper {get; }
        public CourseHelper CourseHelper {get; }
        public GroupHelper GroupHelper {get; }
        public CompetenceHelper CompetenceHelper {get; }

        private EntityProvider() {
            UserHelper = new UserHelper(_accessor);
            ThemeHelper = new ThemeHelper(_accessor);
            SectionHelper = new SectionHelper(_accessor);
            CourseHelper = new CourseHelper(_accessor);
            GroupHelper = new GroupHelper(_accessor);
            CompetenceHelper = new CompetenceHelper(_accessor);
            Tables = new Dictionary<string, dynamic>() {
                { "course", CourseHelper },
                { "section" , SectionHelper},
                { "user" ,  UserHelper},
                { "competence" , CompetenceHelper },
                { "theme" , ThemeHelper},
                { "group" , GroupHelper},
            };
        }

    }
}