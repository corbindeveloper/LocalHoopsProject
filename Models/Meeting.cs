#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CSharpExam.Models;

public class Meeting
{
    [Key]
    public int MeetingId { get; set; }


    // TITLE
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Title:")]
    public string Title { get; set; }

    // DATE
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Date and Time:")]
    [FutureDate]
    public DateTime Date { get; set; }

    // Time
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Duration must be greater than 0.")]
    public int? Time { get; set; }

    // DURATION
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Duration:")]
    public string? Duration { get; set; }


    // DESCRIPTION
    [Required(ErrorMessage = "is required")]
    [Display(Name = "Description:")]
    public string Description { get; set; }

    // CREATED && UPDATED
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    // Total Time
    public string TotalTime()
    {
        return $"{Time} {Duration}";
    }

    // USER
    public int? UserId { get; set; }
    public User? Creator { get; set; }

    // CONNECTION
    public List<Connection> Connections { get; set; } = new List<Connection>();

}