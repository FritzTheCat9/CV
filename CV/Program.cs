using CV;
using Newtonsoft.Json;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Globalization;
using System.Text;

QuestPDF.Settings.License = LicenseType.Community;
QuestPDF.Settings.EnableDebugging = true;
Directory.SetCurrentDirectory("../../../");

var dot = "images/dot.png";
var filePath = "person/person.json";

var readJsonString = File.ReadAllText(filePath, Encoding.UTF8);
var person = JsonConvert.DeserializeObject<Person>(readJsonString);

//var jsonString = JsonConvert.SerializeObject(person, Formatting.Indented);
//File.WriteAllText(filePath, jsonString, Encoding.UTF8);

var fontName = "Rubik";
FontManager.RegisterFont(File.OpenRead(Path.Combine("RubikFont", "Rubik-Regular.ttf")));

var columnMaxWidth = 260;
var imageWidth = 9;

var hugeFontSize = 16;
var bigFontSize = 12;
var boldFontSize = 10;
var smallFontSize = 9;

var secondColor = "384347";

var document = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(1.8f, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(15).FontFamily(fontName));

        page.Content()
            .Column(x =>
            {
                x.Item().Row(x =>
                {
                    GeneratePersonInfo(x, person, imageWidth, secondColor);
                });

                x.Spacing(3);

                x.Item().Row(x =>
                {
                    x.AutoItem().MaxWidth(columnMaxWidth).Column(x =>
                    {
                        x.Spacing(3);

                        AddExperience(x, dot, person);
                        AddEducation(x, dot, person);
                        AddProjects(x, person);
                        AddCourses(x, person);
                    });

                    x.RelativeItem().PaddingLeft(20).AlignRight().Column(x =>
                    {
                        x.Spacing(3);

                        AddAboutMe(x, person);
                        AddSkills(x, person);
                        AddLanguages(x, person);
                        AddHobbies(x, person);
                    });
                });
            });
    });
});

document.GeneratePdf($"person/{person.FirstName}_{person.LastName}_CV.pdf");
document.ShowInPreviewer(12345);

void GeneratePersonInfo(RowDescriptor x, Person person, int imageWidth, string secondColor)
{
    x.AutoItem().Column(x =>
    {
        x.Item().Text($"{person.FirstName} {person.LastName}".ToUpper()).SemiBold().FontSize(26);
        x.Item().Text(person.JobTitle).FontSize(13).SemiBold();

        x.Item().PaddingTop(7).Column(x =>
        {
            if (!string.IsNullOrWhiteSpace(person.Telephone))
            {
                x.Item().Row(x =>
                {
                    GenerateImage(x, person.TelephoneImage, person.Telephone);
                });
            }
            if (!string.IsNullOrWhiteSpace(person.Email))
            {
                x.Item().PaddingTop(3).Row(x =>
                {
                    GenerateImage(x, person.EmailImage, person.Email);
                });
            }
            if (!string.IsNullOrWhiteSpace(person.LinkUrl))
            {
                x.Item().PaddingTop(3).Row(x =>
                {
                    GenerateImage(x, person.LinkImage, person.LinkUrl);
                });
            }
            if (!string.IsNullOrWhiteSpace(person.SecondLinkUrl))
            {
                x.Item().PaddingTop(3).Row(x =>
                {
                    GenerateImage(x, person.SecondLinkImage, person.SecondLinkUrl);
                });
            }
            if (!string.IsNullOrWhiteSpace(person.LinkedinUrl))
            {
                x.Item().PaddingTop(3).Row(x =>
                {
                    GenerateImage(x, person.LinkedinImage, person.LinkedinUrl);
                });
            }
        });
    });

    if (File.Exists(person.Image))
    {
        x.RelativeItem().AlignRight().Width(100).Image(person.Image);
    }
}

void GenerateHeader(ColumnDescriptor x, string name, int hugeFontSize)
{
    x.Item().PaddingTop(5).Text(name.ToUpper()).FontSize(hugeFontSize).SemiBold();
    x.Item().PaddingVertical(0).LineHorizontal(2);
}

void GenerateBigTitle(ColumnDescriptor x, string name, int bigFontSize)
{
    x.Item().AlignMiddle().PaddingVertical(3).Text(name).FontSize(bigFontSize);
}

void GenerateBoldTitle(ColumnDescriptor x, string name, int boldFontSize)
{
    x.Item().AlignMiddle().PaddingVertical(3).Text(name).FontSize(boldFontSize).SemiBold();
}

void AddExperience(ColumnDescriptor x, string dot, Person person)
{
    GenerateHeader(x, "Experience", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var experience in person.Experiences)
        {
            GenerateBigTitle(x, experience.Position, bigFontSize);
            GenerateBoldTitle(x, experience.Organisation, boldFontSize);

            x.Item().Row(x =>
            {
                GenerateDate(x, experience.CalendarImage, experience.StartDate, experience.EndDate);
            });

            foreach (var description in experience.Descrptions)
            {
                x.Item().Row(x =>
                {
                    GenerateDashDescription(x, description, smallFontSize);
                });
            }
        }
    });
}

void GenerateDashDescription(RowDescriptor x, string description, int smallFontSize)
{
    x.AutoItem().AlignMiddle().Width(imageWidth).Image(dot);
    x.AutoItem().MaxWidth(columnMaxWidth - imageWidth).PaddingLeft(imageWidth).Text(description).Justify().FontSize(smallFontSize).FontColor(secondColor);
}

void GenerateDate(RowDescriptor x, string calendarImage, DateTime startDate, DateTime endDate)
{
    x.AutoItem().AlignMiddle().Width(imageWidth).Image(calendarImage);
    x.AutoItem().AlignMiddle().PaddingLeft(imageWidth).Text($"{startDate.ToString("MM/yyyy", CultureInfo.InvariantCulture)} - {endDate.ToString("MM/yyyy", CultureInfo.InvariantCulture)}").FontSize(imageWidth).FontColor(secondColor);
}

void GenerateImage(RowDescriptor x, string image, string text)
{
    x.AutoItem().AlignMiddle().Width(imageWidth).Image(image);
    x.AutoItem().AlignMiddle().PaddingLeft(imageWidth).Text(text).FontSize(imageWidth).FontColor(secondColor);
}

void AddEducation(ColumnDescriptor x, string dot, Person person)
{
    GenerateHeader(x, "Education", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var experience in person.Education)
        {
            GenerateBigTitle(x, experience.Position, bigFontSize);
            GenerateBoldTitle(x, experience.Organisation, boldFontSize);

            x.Item().Row(x =>
            {
                GenerateDate(x, experience.CalendarImage, experience.StartDate, experience.EndDate);
            });

            foreach (var description in experience.Descrptions)
            {
                x.Item().Row(x =>
                {
                    GenerateDashDescription(x, description, smallFontSize);
                });
            }
        }
    });
}

void AddProjects(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "Projects", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var project in person.Projects)
        {
            GenerateBigTitle(x, project.Name, bigFontSize);
            GenerateDescription(x, project.Description, smallFontSize);
            GenerateLink(x, project.Link, smallFontSize);
        }
    });
}

void AddSkills(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "Technical skills", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var group in person.SkillsGroups)
        {
            GenerateBigTitle(x, group.GroupName, bigFontSize);
            x.Spacing(0);
            x.Item().MaxWidth(columnMaxWidth).Inlined(x =>
            {
                x.HorizontalSpacing(7);
                x.AlignLeft();
                x.VerticalSpacing(7);
                foreach (var skill in group.Skills)
                {
                    GenerateSkill(x, boldFontSize, skill);
                }
            });
        }
    });
}

void AddCourses(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "Courses", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var course in person.Courses)
        {
            GenerateBoldTitle(x, course.Name, boldFontSize);
            GenerateLink(x, course.Description, smallFontSize);
        }
    });
}

void AddAboutMe(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "About me", hugeFontSize);
    GenerateDescription(x, person.AboutMe, smallFontSize);
}

void AddLanguages(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "Languages", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var language in person.Languages)
        {
            x.Item().Row(x =>
            {
                x.AutoItem().AlignMiddle().PaddingVertical(3).Text(language.Name).FontSize(boldFontSize).SemiBold();
                x.AutoItem().PaddingVertical(3).PaddingLeft(5).Text("(" + language.Description + ") ").FontSize(smallFontSize).FontColor(secondColor);
            });

        }
    });
}

void AddHobbies(ColumnDescriptor x, Person person)
{
    GenerateHeader(x, "Hobbies", hugeFontSize);

    x.Item().Column(x =>
    {
        foreach (var hobby in person.Hobbies)
        {
            GenerateBoldTitle(x, hobby, boldFontSize);
        }
    });
}

void GenerateDescription(ColumnDescriptor x, string description, int smallFontSize)
{
    x.Item().PaddingVertical(1).Text(description).Justify().FontSize(smallFontSize).FontColor(secondColor);
}

void GenerateLink(ColumnDescriptor x, string description, int smallFontSize)
{
    x.Item().Text(description).FontSize(smallFontSize).FontColor(secondColor);
}

void GenerateSkill(InlinedDescriptor x, int boldFontSize, string skill)
{
    x.Item().Border(0.5f).Padding(5).Text(skill).FontSize(boldFontSize).FontColor(secondColor);
}