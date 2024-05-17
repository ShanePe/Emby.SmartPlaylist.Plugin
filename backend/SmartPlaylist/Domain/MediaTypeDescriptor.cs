using System;
using MediaBrowser.Controller.Entities;
using EmbyMediaTypes = MediaBrowser.Model.Entities.MediaType;
namespace SmartPlaylist.Domain
{

    public class MediaTypeDescriptor
    {
        public string Description { get; }
        public Type MediaType { get; set; }
        public string BaseType { get; set; }
        internal MediaTypeDescriptor(Type type, string baseType, string description)
        {
            MediaType = type;
            BaseType = baseType;
            Description = description;
        }

        private MediaTypeDescriptor() { }

        public static MediaTypeDescriptor CreateAudio<T>() where T : BaseItem
        {
            return MediaTypeDescriptor.CreateAudio<T>(typeof(T).Name);
        }
        public static MediaTypeDescriptor CreateAudio<T>(string description) where T : BaseItem
        {
            return MediaTypeDescriptor.Create<T>(EmbyMediaTypes.Audio, description);
        }
        public static MediaTypeDescriptor CreateVideo<T>() where T : BaseItem
        {
            return MediaTypeDescriptor.CreateVideo<T>(typeof(T).Name);
        }
        public static MediaTypeDescriptor CreateVideo<T>(string description) where T : BaseItem
        {
            return MediaTypeDescriptor.Create<T>(EmbyMediaTypes.Video, description);
        }
        internal static MediaTypeDescriptor Create<T>(string baseType, string description) where T : BaseItem
        {
            return new MediaTypeDescriptor(typeof(T), baseType, description);
        }
    }

}