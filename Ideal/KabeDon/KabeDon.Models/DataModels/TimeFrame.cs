﻿using System;

namespace KabeDon.DataModels
{
    /// <summary>
    /// ある状態をどのくらいの時間表示するか。
    /// </summary>
    public class TimeFrame
    {
        /// <summary>
        /// 平均時間[秒]。
        /// </summary>
        public double AverageSeconds { get; set; }

        /// <summary>
        /// 乱数分布の種類。
        /// </summary>
        public Distribution Distribution { get; set; }

        /// <summary>
        /// 乱数を引いて、平均 <see cref="AverageSeconds"/>、分布 <see cref="Distribution"/> のランダムな時間を1つ得る。
        /// </summary>
        /// <param name="random">一様乱数の生成器。</param>
        /// <returns>乱数を引いた結果。</returns>
        public TimeSpan Next(Random random)
        {
            if (Distribution == Distribution.Constant) return TimeSpan.FromSeconds(AverageSeconds);

            var s = -AverageSeconds * Math.Log(1 - random.NextDouble());
            return TimeSpan.FromSeconds(s);
        }

        /// <summary>
        /// 一定時間。
        /// </summary>
        /// <param name="seconds">秒数。</param>
        /// <returns></returns>
        public static TimeFrame Constant(int seconds) => new TimeFrame { AverageSeconds = seconds, Distribution = Distribution.Constant };

        /// <summary>
        /// 指数分布乱数に沿った時間。
        /// </summary>
        /// <param name="seconds">平均秒数。</param>
        /// <returns></returns>
        public static TimeFrame Exponential(int seconds) => new TimeFrame { AverageSeconds = seconds, Distribution = Distribution.Exponential };
    }
}
