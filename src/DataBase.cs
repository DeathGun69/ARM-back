using System.Collections;
using System.Collections.Generic;

namespace TeacherARMBackend
{
    public class Course
    {
        public int Id {get; set; }
        public string Name {get; set;}
        public int TotalLessons {get; set; }
        public int Lectures {get; set; }
        public int Laboratories {get; set; }
    }

    public class Lesson
    {
        public int Id {get; set; }
        public string Type {get; set; }
        public int CourseId {get; set; }
        public int ThemeId {get; set; } 
    }

    public class Theme
    {
        public int Id {get; set; }
        public string Name {get; set;}
        public int Hours {get; set;}
    }

    public interface ITeacherDataBase
    {
        IEnumerable<Lesson> GetLessons();
        IEnumerable<Theme> GetThemes();
        IEnumerable<Course> GetCourses();
    }

    public class MockDataBase : ITeacherDataBase
    {
        public static MockDataBase DataBase {get; } = new MockDataBase();        

        private List<Course> _courses = new List<Course>();
        private List<Lesson> _lessons = new List<Lesson>();
        private List<Theme> _themes = new List<Theme>();

        private MockDataBase() {
            Theme tFirst = new Theme {
                Id = 0,
                Name = "Первая тема",
                Hours = 30
            };
            Theme tSecond = new Theme {
                Id = 1,
                Name = "Первая тема",
                Hours = 40
            };
            Theme tThird = new Theme {
                Id = 2,
                Name = "Вторая тема",
                Hours = 30
            };
            _themes.AddRange(new List<Theme>{tFirst, tSecond, tThird});

            Course cFirst = new Course{
                Id = 0,
                Name = "Курс 1",
                TotalLessons = 20,
                Lectures = 6,
                Laboratories = 14
            };
            Course cSecond = new Course {
                Id = 1,
                Name = "Курс 2",
                TotalLessons = 40,
                Lectures = 12,
                Laboratories = 28
            };
            _courses.AddRange(new List<Course>{cFirst, cSecond});

            Lesson lFirst = new Lesson {
                Id = 0,
                Type = "Лабораторная",
                ThemeId = 0,
                CourseId = 0
            };
            Lesson lSecond = new Lesson {
                Id = 1,
                Type = "Лекция",
                ThemeId = 0,
                CourseId = 0
            };
            Lesson lThird = new Lesson {
                Id = 2,
                Type = "Лекция",
                ThemeId = 1,
                CourseId = 1
            };
            Lesson lFourth = new Lesson {
                Id = 3,
                Type = "Лекция",
                ThemeId = 1,
                CourseId = 1
            };
            Lesson lFifth = new Lesson {
                Id = 4,
                Type = "Лабораторная",
                ThemeId = 2,
                CourseId = 1
            };
            _lessons.AddRange(new List<Lesson>{lFirst, lSecond, lThird, lFourth, lFifth});
        }
        public IEnumerable<Course> GetCourses() => _courses;

        public IEnumerable<Lesson> GetLessons() => _lessons;

        public IEnumerable<Theme> GetThemes() => _themes;
    }
}