<Query Kind="Statements" />

// Converts .txt [.rtf is pasted into .txt] into .xml file

// Update this as required
string xmlFile = @"C:\Users\bash\Desktop\review.xml";
string file = @"C:\Users\bash\Desktop\review.txt";

// Code - Read no further
var blob = File.ReadAllText(file);

Func<string, string, string[], string> getAttribute = (curr, next, split) => { 
  var betw = split
    .SkipWhile(s => !s.Contains(curr))
    .TakeWhile(s => !s.Contains(next))
    .Where(s => !String.IsNullOrWhiteSpace(s));
  var res = String.Join(" ", betw)
    .Split(new string[] {curr}, StringSplitOptions.None)
    .Last()
    .Trim(new char[] {':', ' '});
  return res;
};

var moviesBlob = blob.Split(new string[] {"Movie Name:", "Movie Name :"}, StringSplitOptions.None);
var movies = moviesBlob
  .Skip(1) // Hack for now
  .Select(m => 
  {
    var split = m.Split(new char[] {'\n','\r'}, StringSplitOptions.None);
    var name = split[0].Trim();
    var link = getAttribute("Review Link", "Review Text", split);
    var review = getAttribute("Review Text", "Reviewer Name", split);
    var rName = getAttribute("Reviewer Name", "Rating", split);
    var rating = getAttribute("Rating", "Affiliation", split).Split(':').Last().Trim();
    var rAff = getAttribute("Affiliation", "Date of Review", split);
    var rDate = getAttribute("Date of Review", "Likes", split);
    var likes = getAttribute("Likes", "\n", split);
   
    return new {
      Name = name,
      Link = link,
      Rating = rating,
      Review = review,
      RName = rName,
      RAffiliation = rAff,
      RDate = rDate,
      Likes = likes,
    };
  })
  .ToList();
 
// movies.Dump(); // Debug

new XDocument(
    new XComment(DateTime.Now.ToString(@"M/d/yyyy hh:mm:ss tt")),
    new XElement("Movies",
        movies.Select(m =>
          new XElement("Movie",
            new XAttribute("Name", m.Name),
            new XAttribute("Link", m.Link),
            new XAttribute("Rating", m.Rating),
            new XElement("Reviews",
              new XElement("Review",
                new XAttribute("Name", m.RName),
                new XAttribute("Affiliation", m.RAffiliation),
                new XAttribute("Date", m.RDate),
                new XAttribute("Likes", m.Likes),
                m.Review
              )
            )
          )
        )
    )
).Save(xmlFile);