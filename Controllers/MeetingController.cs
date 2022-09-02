using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CSharpExam.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CSharpExam.Controllers;

public class MeetingController : Controller
{ 
    // _context is just a variable name -- It can be called anything such as db, or DATABASE
        private CSharpExamContext db;


    // ===========================================================
    // UUID
    private int? uid
    {
        get
        {
            return HttpContext.Session.GetInt32("UUID");
        }
    }


    private bool loggedIn
    {
        get
        {
            return uid != null;
        }
    }

    // here we can "inject" our context service into the constructor
    public MeetingController(CSharpExamContext context)
    {
        db = context;
    }


    // ===========================================================
    // RENDER NEW Meeting FORM
    [HttpGet("/new/meeting")]
    public IActionResult NewMeeting()
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index");
        }

        return View("NewMeeting");
    }


    // ===========================================================
    // SUBMIT NEW Meeting
    [HttpPost("/submit/meeting")]
    public IActionResult submitMeeting(Meeting newMeeting)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid == false) {
            return View("NewMeeting");
        }

        newMeeting.UserId = (int?)uid;
        db.Meetings.Add(newMeeting);
        db.SaveChanges();

        return RedirectToAction("Dashboard", "User");
    }

    // ===========================================================
    // JOIN
    [HttpPost("meeting/Join/{meetingId}")]
    public IActionResult Join(int meetingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index");
        }

        Connection connection = new Connection();
        connection.MeetingId = meetingId;

        if (uid == null)
        {
            return View("Index");
        }
        
        connection.UserId = (int)uid;
        
        
        db.Connections.Add(connection);
        db.SaveChanges();

        // return View("Dashboard");
        return RedirectToAction("Dashboard", "User");
    }

    // ===========================================================
    // LEAVE
    [HttpPost("meeting/Leave/{meetingId}")]
    public IActionResult Leave(int meetingId)
    {
        if (!loggedIn || uid == null)
        {
            return RedirectToAction("Index");
        }

        Connection? connection = db.Connections
            .FirstOrDefault(c => c.UserId == (int)uid && c.MeetingId == meetingId);

        if (connection != null)
        {
            db.Connections.Remove(connection);
            db.SaveChanges();
        }

        // return View("Dashboard");
        return RedirectToAction("Dashboard", "User");
    }

    // ===========================================================
    // DELETE MEETING
    [HttpPost("meeting/deletemeeting/{meetingId}")]
    public IActionResult DeleteMeeting(int meetingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index");
        }

        Meeting? meeting = db.Meetings.FirstOrDefault(meeting => meeting.MeetingId == meetingId);

        if (meeting != null)
        {
            db.Meetings.Remove(meeting);
            db.SaveChanges();
        }    
        return RedirectToAction("Dashboard", "User");
    }


        // ===========================================================
    // VIEW Meeting
    [HttpGet("/view/{MeetingId}")]
    public IActionResult ViewMeeting(int meetingId)
    {
        if (!loggedIn)
        {
            return RedirectToAction("Index");
        }

        Meeting? meeting = db.Meetings
        .Include(m => m.Creator)
        .FirstOrDefault(meeting => meeting.MeetingId == meetingId);

        List<Connection>? guestList = db.Connections
            .Include(connections => connections.User)
            .Include(m => m.User)
            .Where(connection => connection.MeetingId == meetingId)
            .ToList();

        ViewBag.GuestList = guestList;

        return View("ViewMeeting", meeting);
    }






}