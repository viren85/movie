function CallHandler(queryString, OnComp) {
    $.ajax({
        url: queryString,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        responseType: "json",
        cache: false,
        success: OnComp,
        error: OnFail
    });
    return false;
}

function OnFail() { }

function GetQueryStringsForHtmPage() {
    var assoc = {};
    var decode = function (s) { return decodeURIComponent(s.replace(/\+/g, " ")); };
    var queryString = document.location.search.substring(1);

    var keyValues = queryString.split('&');

    for (var i in keyValues) {
        var key = keyValues[i].split('=');
        if (key.length > 1) {
            assoc[decode(key[0])] = decode(key[1]);
        }
    }

    return assoc;
}

function LoadCurrentMovies() {
    var path = "api/Movies?type=current";
    CallHandler(path, onSuccessLoadCurrentMovies);
}

var MOVIES = [];

function onSuccessLoadCurrentMovies(result) {
    result = JSON.parse(result);

    console.log(result);

    if (result.length > 0) {

        MOVIES = result;

        var movieContainer = $("#currentmovie");
        // adding images
        for (var i = 0; i < result.length; i++) {
            var img = $("<img/>")
            img.attr("class", "img-thumbnail");
            img.attr("style", "width: 150px; height: 150px;margin-right: 1%");
            img.attr("data-src", "holder.js/200x200");
            img.attr("alt", result[i].Name);

            var poster = [];
            poster = JSON.parse(result[i].Posters);

            console.log(poster);

            img.attr("src", poster[0].url);

            var anchor = $("<a/>");
            anchor.attr("href", "Movie?movieid=" + result[i].MovieId);
            anchor.attr("title", result[i].Name)
            anchor.append(img);

            movieContainer.append(anchor);
        }

        // adding movie name
        for (var i = 0; i < result.length; i++) {
            var name = $("<div/>");
            name.attr("class", "movie-title");
            name.html("<a href='Movie?movieid=" + result[i].MovieId + "'>" + result[i].Name + "</a>");

            var parent = $(movieContainer).parent();

            parent.append(name);
        }
    }
}

function LoadSingleMovie(movieId) {
    var path = "api/MovieInfo?movieId=" + movieId;

    CallHandler(path, onSuccessLoadSingleMovie);
}

function onSuccessLoadSingleMovie(result) {
    result = JSON.parse(result);

    console.log(result);

    if (result.Movie != undefined) {

        $("#title").html("<span style='font-size: 16px; font-weight: bold'>" + result.Movie.Name + "</span> (" + result.Movie.Year + ")");

        var poster = [];
        poster = JSON.parse(result.Movie.Posters);

        if (poster.length > 0) {
            //showing movies posters
            for (var p = 0; p < poster.length; p++) {
                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 150px; height: 150px;margin-right: 1%");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", result.Movie.Name);
                img.attr("src", poster[p].url);

                $("#posters").append(img);
            }
        }

        $("#genre").html("<b>Genre :</b> " + result.Movie.Genre);
        $("#synopsis").html("<b>Synopsis :</b> " + result.Movie.Synopsis);

        var directors = ""; var writers = ""; var cast = "";

        var casts = [];
        casts = JSON.parse(result.Movie.Casts);

        if (casts.length > 0) {
            for (var c = 0; c < casts.length; c++) {

                if (casts[c].role.toLowerCase() == "director") {
                    directors += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                }
                else if (casts[c].role.toLowerCase() == "writer") {
                    writers += "<a href='javascript:void(0);' title='click here to view profile'>" + casts[c].name + "</a>, ";
                }
                else {
                    cast += "<a href='javascript:void(0);' title='click here to view profile'> " + casts[c].name + "</a>, ";
                }
            }
        }

        if (directors.length > 0) {
            directors = directors.substring(0, directors.lastIndexOf(","));
        }
        if (cast.length > 0) {
            cast = cast.substring(0, cast.lastIndexOf(","));
        }
        if (writers.length > 0) {
            writers = writers.substring(0, writers.lastIndexOf(","));
        }

        $("#director").html("<b>Director :</b> " + directors);
        $("#writer").html("<b>Writer :</b> " + writers);
        $("#cast").html("<b>Cast :</b> " + cast);

        var ratings = [];
        ratings = JSON.parse(result.Movie.Ratings);

        console.log(ratings);
        var rating = 3;
        if (ratings.critic != undefined)
            rating = ratings.critic;

        //$("#rating").html("<b>Rating :</b>  System = " + ratings.system + ", Critics = " + ratings.critic + ", Hot = " + ratings.hot);
        $("#rating").html("<b style='float: left; margin-top: 3%; margin-right: 3%'>Rating :</b><div class='titlePageSprite star-box-giga-star'> " + rating + " </div>");

        // populating Reviews
        var reviews = [];
        //reviews = JSON.parse(result.MovieReviews);
        reviews = result.MovieReviews;

        if (reviews.length > 0) {

            var ul = $("#reviews");

            for (var r = 0; r < reviews.length; r++) {
                var li = $("<li>");
                li.attr("class", "activityContainer");

                var div = $("<div>");
                div.attr("class", "reviewer");

                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 50px; height: 50px;");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", reviews[r].ReviewerName);
                img.attr("src", reviews[r].OutLink);
                img.attr("title", reviews[r].ReviewerName);

                var anchor = $("<a/>");
                anchor.attr("href", "Movie/Reviewer?name=" + reviews[r].ReviewerName);
                anchor.append(img);

                var span = $("<span>");
                span.attr("style", "width:100%; font-weight: bold;");
                span.attr("title", reviews[r].ReviewerName);

                var reviewerName = reviews[r].ReviewerName;

                if (reviews[r].ReviewerName.length > 11) {
                    reviews[r].ReviewerName = reviews[r].ReviewerName.substring(0, 11) + "...";
                }

                span.html("<a href='Movie/Reviewer?name=" + reviewerName + "'>" + reviews[r].ReviewerName + "</a>");
                var br = $("<br/>");
                //div.append(img);
                div.append(anchor);
                div.append(br);
                div.append(span);

                var review = $("<div>");
                review.attr("class", "review");
                if (reviews[r].Review.length > 190) {
                    reviews[r].Review = reviews[r].Review.substring(0, 190) + "...";
                }
                review.html("<a href='Movie/Reviewer?name=" + reviewerName + "' >" + reviews[r].Review + "</a>");

                li.append(div);
                li.append(review);

                ul.append(li);
            }
        }
        else {
            $("#reviews").html("<li class='activityContainer'>No reviews</li>");
        }
    }
}

function GetReviewerAndReviews(name, movie) {
    var path = "../api/ReviewerInfo?name=" + name;

    CallHandler(path, onSuccessPopulateReviewsAndReviews);
}

var ReviewsDetails = [];
var Index = 0;

function onSuccessPopulateReviewsAndReviews(result) {
    result = JSON.parse(result);
    console.log(result);

    if (result != undefined) {
        // adding image
        var img = $("<img/>")
        img.attr("class", "img-thumbnail");
        img.attr("style", "width: 150px; height: 150px;");
        img.attr("data-src", "holder.js/200x200");
        img.attr("alt", result.Name);
        img.attr("src",  result.OutLink);
        img.attr("title", result.Name);
        $("#img").append(img);

        // addmin name
        $("#name").html("<h2>" + result.Name + "</h2>");

        // adding affiliation
        var affiliation = []; affiliation = JSON.parse(result.Affilation);
        if (affiliation != undefined) {
            for (var a = 0; a < affiliation.length; a++) {
                var img = $("<img/>")
                img.attr("class", "img-thumbnail");
                img.attr("style", "width: 50px; height: 50px;margin-right: 1%");
                img.attr("data-src", "holder.js/200x200");
                img.attr("alt", affiliation[a].name);
                img.attr("src", affiliation[a].logoimage);
                img.attr("title", affiliation[a].name);

                var anchor = $("<a/>");
                anchor.attr("href", affiliation[a].link);
                anchor.attr("target", "_blank");
                anchor.append(img);

                $("#affiliation").append(anchor);
            }
        }

        ReviewsDetails = result.ReviewsDetails;

        if (ReviewsDetails != undefined) {
            PopulateReview(ReviewsDetails[0]);
            $("#next").attr("onclick", "Next();");
            $("#previous").attr("onclick", "Previous();");

            if (ReviewsDetails.length == 1) {
                $("#next").hide();
                $("#previous").hide();
            }
            else {
                $("#next").show();
                $("#previous").hide();
            }
        }
    }
}

function PopulateReview(review) {
    $("#moviename").html("<h2>" + review.MovieName + "</h2>");
    $("#rating").html(review.CriticsRating);
    $("#reivew").html(review.Review); 
}

function Next() {
    Index++;
    $("#previous").show();
    if (Index < ReviewsDetails.length) {
        PopulateReview(ReviewsDetails[Index]);
    }

    if (Index == ReviewsDetails.length - 1) {
        $("#next").hide();
        $("#previous").show();
        //Index--;
    }
}

function Previous() {
    Index--;
    $("#next").show();
    if (Index >= 0) {
        PopulateReview(ReviewsDetails[Index]);
    }

    if (Index == 0) {
        $("#previous").hide();
        $("#next").show();
        //Index++;
    }

    return;
}