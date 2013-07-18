using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreLib.Models
{
    class ModelEntity : ITableEntity
    {
#region table members

        public string MovieId {get; set;}
        public string Name {get; set;}
        public string Actors { get; set; }
        public string Directors { get; set; }
        public string Producers { get; set; }
        public string MusicDirectors { get; set; }
        public string ReviewIds { get; set; }
        public string AggregateRating { get; set; }
        public bool HotOrNot { get; set; }
#endregion

        #region GetMethods
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

        #endregion

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

    }
}
