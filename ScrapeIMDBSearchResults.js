// http://www.imdb.com/search/title?at=0&languages=hi%7C1&sort=moviemeter,asc&start=1&title_type=feature

var getmoviesJSON = function () {
	var MovieType = function (el) {
	 var _el = el;
	 
	 var getSafeText = function (el) {
		return el === null ? "" : el.innerText;
	 }
	 
	 return {
		name : function() {
			var titleEl = _el.querySelector(".title");
			var aTags = titleEl !== null ? titleEl.getElementsByTagName("a") : [];
			var aTag = aTags.length >= 2 ? aTags[1] : null;
			return getSafeText(aTag);
		},
		genre : function() {
			return getSafeText(_el.querySelector(".genre"));
		},
		outline : function() {
			return getSafeText(_el.querySelector(".outline"));
		},
		credit : function() {
			return getSafeText(_el.querySelector(".credit"));
		},
	  };
	};

	var Movie = function (el) {
	  var _el = el;
	  
	  var movie = new MovieType(_el);
	  return {
		name: movie.name(),
		genre: movie.genre(),
		outline: movie.outline(),
		credit: movie.credit(),
	  };
	};

	var scrapeMovies = function (el) {
		var movies = new Array();
		var resultsEl = el.querySelector(".results");
		if(resultsEl !== null) {
			var moviesEl = resultsEl.querySelectorAll(".detailed");
			if(resultsEl !== null) {
				for(var m = 0; m < moviesEl.length; ++m) {
				  movies.push(new Movie(moviesEl[m]));
				}
			}
		}
		return movies;
	};

	var movies = scrapeMovies(document.body);
	var moviesJSON = JSON.stringify(movies);
	return moviesJSON;
};

getmoviesJSON();