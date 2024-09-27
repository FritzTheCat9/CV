namespace CV
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }

        public string TelephoneImage { get; set; }
        public string Telephone { get; set; }

        public string EmailImage { get; set; }
        public string Email { get; set; }

        public string LinkUrl { get; set; }
        public string LinkImage { get; set; }

        public string SecondLinkUrl { get; set; }
        public string SecondLinkImage { get; set; }

        public string LinkedinUrl { get; set; }
        public string LinkedinImage { get; set; }

        public string Image { get; set; }

        public List<Experience> Experiences { get; set; }
        public List<Experience> Education { get; set; }
        public List<Project> Projects { get; set; }
        public List<SkillsGroup> SkillsGroups { get; set; }
        public List<Course> Courses { get; set; }

        public string AboutMe { get; set; }

        public List<Language> Languages { get; set; }
        public List<string> Hobbies { get; set; }
    }

    public class Experience
    {
        public string Position { get; set; }
        public string Organisation { get; set; }
        public string CalendarImage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<string> Descrptions { get; set; }
    }

    public class Project
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
    }

    public class SkillsGroup
    {
        public string GroupName { get; set; }
        public List<string> Skills { get; set; }
    }

    public class Course
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class Language
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
