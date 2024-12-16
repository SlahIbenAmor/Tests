using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using database.Models;

namespace database.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
     // Add a private variable of type MyContext (or whatever you named your context file)
    private MyContext _context;         
    // Here we can "inject" our context service into the constructor 
    // The "logger" was something that was already in our code, we're just adding around it 
   
    public HomeController(ILogger<HomeController> logger, MyContext context)    
    {        
        _logger = logger;
        // When our HomeController is instantiated, it will fill in _context with context
        // Remember that when context is initialized, it brings in everything we need from DbContext
        // which comes from Entity Framework Core
        _context = context;    
    }    

    [HttpGet("")]    
    public IActionResult Index()    
    {     
        // Now any time we want to access our database we use _context   
        List<Song> AllSongs = _context.Songs.ToList();
        return View();    
    } 

    [HttpPost("songs/create")]
    public IActionResult Create(Song newSong)
    {
        if(ModelState.IsValid)
        {
        _context.Add(newSong);
        _context.SaveChanges();
        return RedirectToAction("AllSongs");
        }else {

            return View("Index");
        }
    }

    [HttpGet("Songs")]    
    public IActionResult AllSongs()    
    {     
        List<Song>AllSongs=_context.Songs.ToList();
        return View("AllSongs", AllSongs);    
    } 

    [HttpPost("songs/{SongId}/destroy")]
    public IActionResult Destroy(int SongId)
    {
        Song? SongToDelete = _context.Songs.SingleOrDefault(i => i.SongId == SongId);
        // Once again, it could be a good idea to verify the monster exists before deleting
        _context.Songs.Remove(SongToDelete);
        _context.SaveChanges();
        return RedirectToAction("AllSongs");
    }

    [HttpPost("songs/{SongId}/update")]
    public IActionResult Update(int SongId, Song newSong)
    {
        // 2. Find the old version of the instance in your database
        Song? OldSong = _context.Songs.FirstOrDefault(i => i.SongId == SongId);
        // 3. Verify that the new instance passes validations
        if(ModelState.IsValid)
        {
            // 4. Overwrite the old version with the new version
            // Yes, this has to be done one attribute at a time
            OldSong.Title = newSong.Title;
            OldSong.Year = newSong.Year;
            OldSong.Genre = newSong.Genre;
            // You updated it, so update the UpdatedAt field!
            OldSong.UpdatedAt = DateTime.Now;
            // 5. Save your changes
            _context.SaveChanges();
            // 6. Redirect to an appropriate page
            return RedirectToAction("AllSongs");
        } else {
            // 3.5. If it does not pass validations, show error messages
            // Be sure to pass the form back in so you don't lose your changes
            // It should be the old version so we can keep the ID
            return View("Edit", OldSong);
        }
    }

    [HttpGet("songs/{SongId}/edit")]    
    public IActionResult Edit(int SongId)    
    {     
        Song? OldSong = _context.Songs.FirstOrDefault(i => i.SongId == SongId);
        return View("Edit", OldSong);    
    } 
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
