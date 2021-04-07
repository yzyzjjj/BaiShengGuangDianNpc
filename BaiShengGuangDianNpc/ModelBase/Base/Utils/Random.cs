using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;

namespace ModelBase.Base.Utils
{
	public static class RandomSeed
	{
		private static readonly Random _Seed = new Random();

		public static IEnumerable<Tuple<TTarget, TSource>> NextGroup<TSource, TTarget>(IList<TSource> sources,
			IEnumerable<TTarget> targets, short amount)
		{
			var ret = new List<Tuple<TTarget, TSource>>();
			var t = targets.ToList();
			var scount = sources.Count;
			var tcount = t.Count;
			for (var i = 0; i < amount; i++)
			{
				var grid = _Seed.Next(tcount--);
				ret.Add(new Tuple<TTarget, TSource>(t[grid], sources[_Seed.Next(scount)]));
				t.RemoveAt(grid);
			}
			return ret;
		}


		/// <summary>
		///     摘要:
		///     返回非负随机数。
		///     返回结果:
		///     大于等于零且小于 System.Int32.MaxValue 的 32 位带符号整数。
		/// </summary>
		/// <returns></returns>
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static int Next()
		{
			return _Seed.Next();
		}

		/// <summary>
		///     摘要:
		///     返回一个小于所指定最大值的非负随机数。
		///     参数:
		///     maxValue:
		///     要生成的随机数的上限（随机数不能取该上限值）。maxValue 必须大于或等于零。
		///     返回结果:
		///     大于等于零且小于 maxValue 的 32 位带符号整数，即：返回值的范围通常包括零但不包括 maxValue。不过，如果 maxValue 等于零，则返回
		///     maxValue。
		///     异常:
		///     System.ArgumentOutOfRangeException:
		///     maxValue 小于零。
		/// </summary>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int Next(int maxValue)
		{
			return _Seed.Next(maxValue);
		}


		/// <summary>
		///     摘要:
		///     返回一个指定范围内的随机数。
		///     参数:
		///     minValue:
		///     返回的随机数的下界（随机数可取该下界值）。
		///     maxValue:
		///     返回的随机数的上界（随机数不能取该上界值）。maxValue 必须大于或等于 minValue。
		///     返回结果:
		///     一个大于等于 minValue 且小于 maxValue 的 32 位带符号整数，即：返回的值范围包括 minValue 但不包括 maxValue。如果
		///     minValue 等于 maxValue，则返回 minValue。
		///     异常:
		///     System.ArgumentOutOfRangeException:
		///     minValue 大于 maxValue。
		/// </summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public static int Next(int minValue, int maxValue)
		{
			return _Seed.Next(minValue, maxValue);
		}

		//
		// 摘要:
		//     用随机数填充指定字节数组的元素。
		//
		// 参数:
		//   buffer:
		//     包含随机数的字节数组。
		//
		// 异常:
		//   System.ArgumentNullException:
		//     buffer 为 null。
		public static void NextBytes(byte[] buffer)
		{
			_Seed.NextBytes(buffer);
		}

		//
		// 摘要:
		//     返回一个介于 0.0 和 1.0 之间的随机数。
		//
		// 返回结果:
		//     大于等于 0.0 并且小于 1.0 的双精度浮点数。
		[TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
		public static double NextDouble()
		{
			return _Seed.NextDouble();
		}

		/// <summary>
		///     获取N个的随机数
		/// </summary>
		/// <param name="minValue">下限，取到</param>
		/// <param name="maxValue">上限，取不到</param>
		/// <param name="count">N</param>
		/// <returns></returns>
		public static IEnumerable<int> NextN(int minValue, int maxValue, int count)
		{
			var rets = new HashSet<int>();
			if (maxValue - minValue < count)
				count = maxValue - minValue;
			while (rets.Count < count)
				rets.Add(Next(minValue, maxValue));
			return rets;
		}

		/// <summary>
		///     获取N个不同的随机数，排除掉部分数
		/// </summary>
		/// <param name="minValue">下限，取到</param>
		/// <param name="maxValue">上限，取不到</param>
		/// <param name="count">数量N</param>
		/// <param name="excepts">排除的数</param>
		/// <returns></returns>
		public static IEnumerable<int> NextN(int minValue, int maxValue, int count, IEnumerable<int> excepts)
		{
			var rets = new HashSet<int>();
			foreach (var i in excepts)
				if (i >= minValue && i < maxValue)
					rets.Add(i);
			var rc = rets.Count;
			if (maxValue - minValue - rc < count)
				count = maxValue - minValue - rc;
			while (rets.Count - rc < count)
				rets.Add(Next(minValue, maxValue));
			return rets.Except(excepts);
		}

		/// <summary>
		///     根据权重随机
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="elements"></param>
		/// <returns></returns>
		public static IWeightRandom GetWeightRandom(IEnumerable<IWeightRandom> elements)
		{
			var totWeight = elements.Sum(w => w.Weight);
			var randomNumber = Next(totWeight);
			var tot = 0;
			for (var i = 0; i < elements.Count(); ++i)
			{
				tot += elements.ElementAt(i).Weight;
				if (randomNumber < tot)
					return elements.ElementAt(i);
			}
			return null;
		}

		public static int GetWeightRandom(IEnumerable<Tuple<int, int>> elements)
		{
			var totWeight = elements.Sum(w => w.Item2);
			var randomNumber = Next(totWeight);
			var tot = 0;
			for (var i = 0; i < elements.Count(); ++i)
			{
				tot += elements.ElementAt(i).Item2;
				if (randomNumber < tot)
					return elements.ElementAt(i).Item1;
			}
			return 0;
		}

		public static IEnumerable<IWeightRandom> GetWeightRandoms(
			IEnumerable<IWeightRandom> elements, int n = 1, bool repeat = false)
		{
			if ((!repeat && n > elements.Count()) || n < 0)
			{
				return null;
			}
			var ret = new List<IWeightRandom>();
			var list = elements.ToList();
			int totalWeight = elements.Sum(item => item.Weight);
			int step = repeat ? 0 : 1;
			for (int i = 0; i < list.Count() && ret.Count() < n; i += step)
			{
				var ranNumber = Next(totalWeight);
				for (int j = i; j < list.Count(); ++j)
				{
					ranNumber -= list[j].Weight;
					if (ranNumber >= 0)
						continue;

					ret.Add(list[j]);
					var newItem = list[j];
					list[j] = list[i];
					list[i] = newItem;
					totalWeight -= (repeat ? 0 : newItem.Weight);
					break;
				}
			}
			return ret;
		}

		/// <summary>
		/// 随机一个对象
		/// 说明: 若为独立概率的情况下, 前几个概率和达到100%, 则后几个的实际概率将变为0
		/// </summary>
		/// <typeparam name="T">待随机对象的类型</typeparam>
		/// <param name="list">T => rate</param>
		/// <param name="bJoint">true:联合概率(基数取各自概率之和), false:独立概率(基数取100)</param>
		/// <returns></returns>
		public static T GetRandom<T>(IEnumerable<Tuple<T, int>> list, bool bJoint = false)
		{
			if (!list.Any())
				return default(T);

			var total = bJoint ? list.Sum(x => x.Item2) : 100;		// 总概率
			var r = Next(total);									// 随机值
			//Log.InfoFormat("zzb103: r={0}, total={1}", r, total);
			var rate = 0;
			for (var i = 0; i < list.Count(); i++)
			{
				var item = list.ElementAt(i);
				rate += item.Item2;
				if (r < rate)
					return item.Item1;								// 找到对应的项
			}

			return default(T);										// 找不到
		}
	}

	public interface IWeightRandom
	{
		int Weight { get; }
	}


	/// <summary>
	///     系统默认随机盒,应对一个集合内的百分率计算问题
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public static class RandomBox<T>
	{
		private static int actualPower;
		private static int _currentValue;
		private static readonly List<Tuple<T, int>> _Seeds = new List<Tuple<T, int>>();

		public static void AddSeed(T _Seed, int _Power)
		{
			actualPower += _Power;

			_Seeds.Add(new Tuple<T, int>(_Seed, actualPower));
		}

		private static void ClearSeed()
		{
			actualPower = 0;
			_Seeds.Clear();
		}

		public static T TrigRandom()
		{
			var _tmpSeed = RandomSeed.Next(1, actualPower + 1);
			_currentValue = _tmpSeed;
			foreach (var keyValuePair in _Seeds)
				if (keyValuePair.Item2 >= _tmpSeed)
				{
					ClearSeed();
					return keyValuePair.Item1;
				}
			ClearSeed();
			return default(T);
		}
	}
}