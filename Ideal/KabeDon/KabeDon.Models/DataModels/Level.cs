﻿using System.Collections.Generic;

namespace KabeDon.DataModels
{
    /// <summary>
    /// ステージ データ。
    /// </summary>
    public class Level
    {
        /// <summary>
        /// 制限時間[秒]
        /// </summary>
        public int TimeLimitSeconds { get; set; }

        /// <summary>
        /// 状態一覧。
        /// </summary>
        public List<State> States { get; } = new List<State>();

        /// <summary>
        /// 初期状態の ID。
        /// </summary>
        public string InitialState { get; set; }

        /// <summary>
        /// 開始音声のファイル名。
        /// </summary>
        public string StartSound { get; set; }

        /// <summary>
        /// 終了音声のファイル名。
        /// </summary>
        public string FinishSound { get; set; }

        /// <summary>
        /// 初期ステートを取得。
        /// </summary>
        /// <returns></returns>
        public State GetInitialState() => States.Find(x => x.Id == InitialState);

        /// <summary>
        /// ID を指定してステートを取得。
        /// </summary>
        /// <returns></returns>
        public State GetState(string id) => States.Find(x => x.Id == id);
    }
}
