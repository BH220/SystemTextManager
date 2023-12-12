using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemTextManager.Launcher
{ 
    public class FileModel
    {
        /// <summary>
        /// 프로그램 시작 경로 기준으로한 상대경로
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 파일 크기
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// 파일의 버전
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// MD5 해시값
        /// </summary>
        public string MD5 { get; set; }
    }
}
