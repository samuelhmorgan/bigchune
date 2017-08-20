using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BigChune.Data.Templates
{
    public class AllMusicTemplate : AbstractTemplate
    {
        private string _whereClause = string.Empty;

        private string _template =
            "MATCH (music:SoundFile) {whereClause} RETURN music.title as title, music.artist as artist, music.album as album, music.genre as genre," +
            " music.directory as directory, music.filename as filename, music.hash as hash,  music.mime as mime, music.albumart as albumart LIMIT 10000";

        public override string Query
            => _template.Replace("{whereClause}", _whereClause);

        public AllMusicTemplate StartsWith(string s)
        {
            _whereClause = $" WHERE music.title =~ '{s}.*' ";
            return this;
        }

        public AllMusicTemplate Find(string s)
        {
            _whereClause = $" WHERE music.hash =~ '{s}.*' ";
            return this;
        }
    }
}
