using FirstBlazorApp.Data;
using ForumAdminPanel.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FirstBlazorApp.Services
{
    public class AnswerService
    {   
        IDbContextFactory<appDbContext> _dbContextFactory;

        public AnswerService(IDbContextFactory<appDbContext> dbContextFactory) 
        {
            _dbContextFactory = dbContextFactory;
        }

        // List answers for single post 
        public List<Answer> GetAnswersForPost(int postId)
        {   
            var answers = new List<Answer>();
            using(var context = _dbContextFactory.CreateDbContext())
            {   
                bool answersExist =context.Answers.Any(A=> A.PostId== postId);
                var postAnswers = context.Answers.Where(A => A.PostId == postId).ToList();

                if(answersExist)
                {
                    foreach (var answer in postAnswers) 
                    {
                        answers.Add(answer);
                    }
                }
                else
                {
                    return null;
                }
            }
            return answers;
        }

        // Create answer for single post
        public Answer CreateAnswer(int postId, int userId, Answer answerInput)
        {
                       
            using(var context = _dbContextFactory.CreateDbContext())
            {
                Post post = context.Posts.Where(P => P.Id == postId).FirstOrDefault();
                User user = context.Users.Where(U=>U.Id == userId).FirstOrDefault();

                Answer answer = new Answer()
                {
                    Body = answerInput.Body,
                    UserId = userId,
                    PostId = postId,
                    TimeAnswerCreated = DateTime.UtcNow,
                };

                context.Answers.Add(answer);
                context.SaveChanges();

                return answer;
            }

        }
    }


}
