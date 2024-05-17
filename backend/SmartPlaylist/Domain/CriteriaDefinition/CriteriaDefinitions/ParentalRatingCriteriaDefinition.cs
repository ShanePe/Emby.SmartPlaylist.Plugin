using System;
using System.Linq;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Model.Entities;
using SmartPlaylist.Domain.Rule;
using SmartPlaylist.Domain.Values;

namespace SmartPlaylist.Domain.CriteriaDefinition.CriteriaDefinitions
{
    public class ParentalRatingCriteriaDefinition : CriteriaDefinition
    {
        public override string Name => "Parental Rating";
        public override CriteriaDefinitionType Type => new ListValueDefinitionType(Getter.OfficialRatings.First());
        public override Value[] Values { get; } = Getter.OfficialRatings;
        public override Value GetValue(UserItem item)
        {
            var rating = item.Item.OfficialRating;
            if (string.IsNullOrEmpty(rating)) return Value.None;

            return ListValue.Create(rating);
        }


    }
}