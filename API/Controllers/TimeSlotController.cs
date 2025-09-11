using Microsoft.AspNetCore.Mvc;
using Models.Data;
using Models.Models;
using static Models.Data.DB;

namespace API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class TimeSlotController : Controller
{

    private readonly DB _db;

    public TimeSlotController(DB db)
    {
        _db = db;
    }


    [HttpGet]
    [Route("/api/getTimeslots")]
    public IActionResult GetTimeSlots()
    {
        string query ="SELECT * from TimeSlot";
        var dt = _db.FillAndReturnDataTable(query);

        List<TimeSlot> timeSlots = new List<TimeSlot>();

        foreach (System.Data.DataRow row in dt.Rows)
        {
            timeSlots.Add(new TimeSlot
            {
                TimeSlotId = row["TimeSlotId"].ToString()!,
                StartTime = TimeOnly.Parse(row["StartTime"].ToString()!),
                EndTime = TimeOnly.Parse(row["EndTime"].ToString()!),
                IsAvailable = Convert.ToBoolean(row["IsAvailable"])
            });
        }

        return Ok(timeSlots);

    }

    [HttpGet]
    [Route("api/getTimeslotsByTimeSlotId/{TimeSlotId}")]
    public IActionResult GetTimeSlotsById(string TimeSlotId)
    {
        string query = "SELECT TimeSlotId, StartTime, EndTime, IsAvailable FROM TimeSlot WHERE TimeSlotId = @TimeSlotId";

        var parameters = new nameValuePairList
    {
        new nameValuePair("@TimeSlotId", TimeSlotId)
    };

        var dt = _db.FillAndReturnDataSet(query, parameters);

        List<TimeSlot> timeSlots = new List<TimeSlot>();

        foreach (System.Data.DataRow row in dt.Rows)
        {
            timeSlots.Add(new TimeSlot
            {
                TimeSlotId = row["TimeSlotId"].ToString()!,
                StartTime = TimeOnly.Parse(row["StartTime"].ToString()!),
                EndTime = TimeOnly.Parse(row["EndTime"].ToString()!),
                IsAvailable = Convert.ToBoolean(row["IsAvailable"])
            });
        }

        return Ok(timeSlots);
    }

}
