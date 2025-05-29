using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcE_Learning.Models;

public class CourseViewModel
{
    public List<Course>? Course { get; set; }
    public SelectList? Fields { get; set; }
    public string? CourseFields { get; set; }
    public string? SearchString { get; set; }
}