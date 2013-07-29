using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace DataStoreLib.Models
{
    public class MovieEntity : TableEntity
    {
#region table members
        public static readonly string PARTITION_KEY = "CloudMovie";

        public string MovieId {get; set;}
        public string Name {get; set;}
        public string Actors { get; set; }
        public string Directors { get; set; }
        public string Producers { get; set; }
        public string MusicDirectors { get; set; }
        public string ReviewIds { get; set; }
        public string AggregateRating { get; set; }
        public bool HotOrNot { get; set; }

        public override void ReadEntity(IDictionary<string, EntityProperty> properties,
                                       Microsoft.WindowsAzure.Storage.OperationContext operationContext)
        {
            base.ReadEntity(properties, operationContext);

            MovieId = ReadString(properties, "MovieId");
            Name = ReadString(properties, "Name");
            Actors = ReadString(properties, "Actors");
            Directors = ReadString(properties, "Directors");
            Producers = ReadString(properties, "Producers");
            MusicDirectors = ReadString(properties, "MusicDirectors");
            ReviewIds = ReadString(properties, "ReviewIds");
            AggregateRating = ReadString(properties, "AggregateRating");
            HotOrNot = ReadBool(properties, "HotOrNot");
        }
#endregion

        public MovieEntity()
            : base(PARTITION_KEY, "")
        {
            
        }

        public MovieEntity(string rowKey)
            : base(PARTITION_KEY, rowKey)
        {
        }

        public MovieEntity(MovieEntity entity)
            : base(PARTITION_KEY, entity.RowKey)
        {
            MovieId = entity.MovieId;
            Name = entity.Name;
            Actors = entity.Actors;
            Directors = entity.Directors;
            Producers = entity.Producers;
            MusicDirectors = entity.MusicDirectors;
            ReviewIds = entity.ReviewIds;
            AggregateRating = entity.AggregateRating;
            HotOrNot = entity.HotOrNot;
        }

        public static MovieEntity CreateMovieEntity(string name,
                                                    string actors,
                                                    string directors,
                                                    string producers,
                                                    string musicDirecotrs,
                                                    string reviewIds,
                                                    string aggregateRating,
                                                    bool hotOrNot)
        {
            var movieId = Guid.NewGuid().ToString();
            var entity = new MovieEntity(movieId);
            entity.MovieId = movieId;
            entity.Name = name;
            entity.Actors = actors;
            entity.Directors = directors;
            entity.Producers = producers;
            entity.MusicDirectors = musicDirecotrs;
            entity.ReviewIds = reviewIds;
            entity.AggregateRating = aggregateRating;
            entity.HotOrNot = hotOrNot;

            return entity;
        }

        #region AccessMethods
        public List<string> GetActors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Actors);
        }

        public List<string> GetDirectors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Directors);
        }

        public List<string> GetProducers()
        {
            return Utils.utils.GetListFromCommaSeparatedString(Producers);
        }

        public List<string> GetMusicDirectors()
        {
            return Utils.utils.GetListFromCommaSeparatedString(MusicDirectors);
        }

        public List<string> GetReviewIds()
        {
            return Utils.utils.GetListFromCommaSeparatedString(ReviewIds);
        } 

        public void SetActors(List<string> list)
        {
            Actors = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetDirectors(List<string> list)
        {
            Directors = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetMusicDirectors(List<string> list)
        {
            MusicDirectors = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        public void SetReviewIds(List<string> list)
        {
            ReviewIds = Utils.utils.GetCommaSeparatedStringFromList(list);
        }

        #endregion
    }
}
