using System;
using System.Collections.Generic;
using System.Linq;
using DataStorage.Serialization;

namespace DataStorage.Services
{
    public interface IBestScoresService
    {
        IEnumerable<BestScore> GetBestScores();
        void Update(int userId, int score);
    }

    public class BestScoresService : IBestScoresService
    {
        private readonly IBestScoreSerializer _bestScoreSerializer;
        private readonly IUserFinder _userFinder;

        public BestScoresService(IBestScoreSerializer bestScoreSerializer, IUserFinder userFinder)
        {
            _bestScoreSerializer = bestScoreSerializer;
            _userFinder = userFinder;
        }

        public IEnumerable<BestScore> GetBestScores()
        {
            var bestScores = _bestScoreSerializer.Deserialize() ?? new List<BestScore>();
            foreach (var bestScore in bestScores)
            {
                var user = _userFinder.Find(bestScore.UserId);
                if (user != null) bestScore.UserName = user.Name;
            }

            return bestScores;
        }

        public void Update(int userId, int score)
        {
            var bestScores = _bestScoreSerializer.Deserialize() ?? new List<BestScore>();
            if (bestScores.Count < 10 || score > bestScores.Last().Score)
            {
                var bestScore = new BestScore
                {
                    UserId = userId,
                    Score = score,
                };

                UpdatePositions(bestScores, bestScore);
                _bestScoreSerializer.Serialize(bestScores);
            }
        }

        private void UpdatePositions(IEnumerable<BestScore> bestScores, BestScore bestScore)
        {
            var list = bestScores as List<BestScore> ?? bestScores.ToList();
            list.Add(bestScore);
            list.Sort((score, score1) =>
            {
                if (score.Score < score1.Score) return 1;
                if (score.Score > score1.Score) return -1;
                return 0;
            });
            if(list.Count > 10) list.RemoveAt(10);
        }
    }
}