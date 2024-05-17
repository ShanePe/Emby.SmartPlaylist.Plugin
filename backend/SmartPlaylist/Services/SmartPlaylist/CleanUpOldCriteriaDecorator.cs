using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SmartPlaylist.Contracts;
using SmartPlaylist.Domain;
using SmartPlaylist.Domain.CriteriaDefinition;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Services.SmartPlaylist
{
    public class CleanupOldCriteriaDecorator : ISmartPlaylistStore

    {
        private readonly ISmartPlaylistStore _decorated;

        public CleanupOldCriteriaDecorator(ISmartPlaylistStore decorated)
        {
            _decorated = decorated;
        }

        public async Task<SmartPlaylistDto> GetSmartPlaylistAsync(Guid smartPlaylistId)
        {
            var dto = await _decorated.GetSmartPlaylistAsync(smartPlaylistId).ConfigureAwait(false);

            CleanupSmartPlaylist(dto);

            return dto;
        }

        public async Task<SmartPlaylistDto[]> LoadPlaylistsAsync(Guid userId)
        {
            var smartPlaylists = await _decorated.LoadPlaylistsAsync(userId).ConfigureAwait(false);

            smartPlaylists.ToList().ForEach(CleanupSmartPlaylist);

            return smartPlaylists;
        }

        public async Task<SmartPlaylistDto[]> GetAllSmartPlaylistsAsync()
        {
            var smartPlaylists = await _decorated.GetAllSmartPlaylistsAsync().ConfigureAwait(false);

            smartPlaylists.ToList().ForEach(CleanupSmartPlaylist);

            return smartPlaylists;
        }

        public void Save(SmartPlaylistDto smartPlaylist)
        {
            _decorated.Save(smartPlaylist);
        }

        public void Delete(Guid userId, string smartPlaylistId)
        {
            _decorated.Delete(userId, smartPlaylistId);
        }

        private void CleanupSmartPlaylist(SmartPlaylistDto dto)
        {
            if (dto == null)
                return;

            var limitChanged = CleanupOldLimit(dto);
            var criteriaChanged = CleanupOldCriteria(dto);

            if (limitChanged || criteriaChanged) _decorated.Save(dto);
        }

        private static bool CleanupOldCriteria(SmartPlaylistDto dto)
        {
            var changed = false;
            var rulesWithCriteria = dto.RulesTree.Where(x => x.Data?.Criteria != null).ToArray();
            foreach (var rule in rulesWithCriteria)
            {
                var criteria = rule.Data.Criteria;
                var criteriaDefinition = DefinedCriteriaDefinitions.All.FirstOrDefault(x => x.Name == criteria.Name);
                if (criteriaDefinition == null)
                {
                    ChangeCriteriaDefinition(criteria);

                    changed = true;
                }
                else if (IsOldListOrMapValue(criteria.Value, criteriaDefinition.Values))
                {
                    criteria.Value = criteriaDefinition.Values.FirstOrDefault();
                    changed = true;
                }
            }

            return changed;
        }

        private static void ChangeCriteriaDefinition(RuleCriteriaValueDto criteria)
        {
            var criteriaDefinition = GetFirstBestMatchedCriteriaDef(criteria);
            var criteriaDefOperator = criteriaDefinition.Type.Operators.FirstOrDefault();
            criteria.Name = criteriaDefinition.Name;
            if (criteria.Operator.Type != criteriaDefOperator.Type)
            {
                criteria.Operator = criteriaDefOperator.ToDto();
                criteria.Value = criteriaDefOperator.DefaultValue;
            }
        }

        private static CriteriaDefinition GetFirstBestMatchedCriteriaDef(RuleCriteriaValueDto criteria)
        {
            return DefinedCriteriaDefinitions.All.FirstOrDefault(x => x.Type.Name == criteria.Operator.Type) ??
                   DefinedCriteriaDefinitions.All.FirstOrDefault();
        }

        private static bool CleanupOldLimit(SmartPlaylistDto dto)
        {
            if (dto == null || dto.Limit == null)
                return false;

            var changed = false;
            if (dto.Limit.HasLimit && !DefinedOrders.AllNames.Contains(dto.Limit.OrderBy))
            {
                dto.Limit.OrderBy = DefinedOrders.AllNames.FirstOrDefault();
                changed = true;
            }

            return changed;
        }

        private static bool IsOldListOrMapValue(object currentValue, Value[] availableValues)
        {
            return currentValue is ListValue listValue && availableValues.Any() && !availableValues.Contains(listValue)
            || currentValue is ListMapValue listMapValue && availableValues.Any() && !availableValues.Contains(listMapValue);
        }

        public bool Exists(Guid userId, string smartPlaylistId)
        {
            return _decorated.Exists(userId, smartPlaylistId);
        }

        public async Task WriteToLogAsync(Domain.SmartPlaylist smartPlaylist)
        {
            await _decorated.WriteToLogAsync(smartPlaylist);
        }

        public Stream GetLogFileStream(Guid userId, string smartPlaylistId)
        {
            return _decorated.GetLogFileStream(userId, smartPlaylistId);
        }

        public string GetLogFilePath(Guid userId, string smartPlaylistId)
        {
            return _decorated.GetLogFilePath(userId, smartPlaylistId);
        }

        public async Task<string> ExportAsync(string[] smartPlaylistIds)
        {
            return await _decorated.ExportAsync(smartPlaylistIds).ConfigureAwait(false);
        }

        public void Delete(string path)
        {
            _decorated.Delete(path);
        }

        public async Task<string> ImportAsync(byte[] fileData, Guid userId)
        {
            return await _decorated.ImportAsync(fileData, userId).ConfigureAwait(false);
        }
    }
}
