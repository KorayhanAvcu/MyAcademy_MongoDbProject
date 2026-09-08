
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Travel.Web.DTOs.QuestionDtos;
using Travel.Web.Entities;
using Travel.Web.Entities.Tour;
using Travel.Web.Settings;

namespace Travel.Web.Services.QuestionServices
{
    public class QuestionService : IQuestionService
    {
        private readonly IMongoCollection<Question> _questionCollection;
        private readonly IMongoCollection<Tour> _tourCollection;
        private readonly IMapper _mapper;

        public QuestionService(
            IDatabaseSettings databaseSettings,
            IMapper mapper)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _questionCollection = database.GetCollection<Question>(
                databaseSettings.QuestionCollectionName);

            _tourCollection = database.GetCollection<Tour>(
                databaseSettings.TourCollectionName);

            _mapper = mapper;
        }

        public async Task<List<QuestionListDto>> GetAllAsync()
        {
            var questions = await _questionCollection
                .AsQueryable()
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();

            var result = _mapper.Map<List<QuestionListDto>>(questions);

            foreach (var question in result)
            {
                var tour = await _tourCollection
                    .Find(x => x.Id == question.TourId)
                    .FirstOrDefaultAsync();

                if (tour != null)
                    question.TourName = tour.Name;
            }

            return result;
        }

        public async Task<QuestionDetailDto> GetByIdAsync(string id)
        {
            var question = await _questionCollection
                .Find(x => x.Id == id)
                .FirstOrDefaultAsync();

            if (question == null)
                return null;

            var result = _mapper.Map<QuestionDetailDto>(question);

            var tour = await _tourCollection
                .Find(x => x.Id == question.TourId)
                .FirstOrDefaultAsync();

            if (tour != null)
                result.TourName = tour.Name;

            return result;
        }

        public async Task CreateAsync(CreateQuestionDto createQuestionDto)
        {
            var question = _mapper.Map<Question>(createQuestionDto);

            question.IsRead = false;
            question.IsAnswered = false;
            question.Answer = null;
            question.AnsweredAt = null;

            await _questionCollection.InsertOneAsync(question);
        }

        public async Task AnswerAsync(AnswerQuestionDto answerQuestionDto)
        {
            var update = Builders<Question>.Update
                .Set(x => x.Answer, answerQuestionDto.Answer)
                .Set(x => x.IsAnswered, true)
                .Set(x => x.AnsweredAt, DateTime.UtcNow)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _questionCollection.UpdateOneAsync(
                x => x.Id == answerQuestionDto.Id,
                update);
        }

        public async Task MarkAsReadAsync(string id)
        {
            var update = Builders<Question>.Update
                .Set(x => x.IsRead, true)
                .Set(x => x.UpdatedAt, DateTime.UtcNow);

            await _questionCollection.UpdateOneAsync(
                x => x.Id == id,
                update);
        }

        public async Task DeleteAsync(string id)
        {
            await _questionCollection.DeleteOneAsync(
                x => x.Id == id);
        }
        public async Task<int> GetUnreadCountAsync() 
        { 
            return (int)await _questionCollection.CountDocumentsAsync(x => !x.IsRead); 
        }
    }
}

