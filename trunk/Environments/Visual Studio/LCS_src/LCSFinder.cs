/*
Author: Thanh Ngoc Dao
Copyright (c) 2005 by Thanh Ngoc Dao.
*/

using System;
using System.Diagnostics;

namespace LCS
{
	/// <summary>
	/// Summary description for LCSFinder.
	/// </summary>
	public class LCSFinder
	{
		public LCSFinder()
		{
		}
		
		public enum BackTracking {
			NEITHER,
			UP,
			LEFT,
			UP_AND_LEFT
		}

		public static string GetLCS(string s, string t)
		{
			Tokeniser tok=new Tokeniser() ;
			string[] ss=tok.Partition(s) ;
			string[] tt=tok.Partition(t) ;

			string str=LCS (ss, tt);
			return str;
		}

		private static int ConsecutiveMeasure(int k)
		{
			//f(k)=k*a - b;
			return k*k;
		}

		private static string LCS(string[] list1, string[] list2) 
		{
			int m=list1.Length ;
			int n=list2.Length ;
			
			int[ , ] lcs=new int[m+1, n+1];
			BackTracking[ , ] backTracer=new BackTracking[m+1, n+1];
			int[ , ] w=new int[m+1, n+1];
			int i, j;
			
			for(i=0; i <= m; ++i) 
			{
				lcs[i,0] = 0;
				backTracer[i,0]=BackTracking.UP;
				
			}
			for(j= 0; j <= n; ++j) 
			{
				lcs[0,j]=0;
				backTracer[0,j]=BackTracking.LEFT;				
			}

			for(i =1; i <= m; ++i) 
			{
				for(j=1; j <= n; ++j) 
				{ 
					if( list1[i-1].Equals(list2[j-1]) ) 
					{
						int k = w[i-1, j-1];
						//lcs[i,j] = lcs[i-1,j-1] + 1;
						lcs[i,j]=lcs[i-1,j-1] + ConsecutiveMeasure(k+1) - ConsecutiveMeasure(k)  ;
						backTracer[i,j] = BackTracking.UP_AND_LEFT;
						w[i,j] = k+1;						
					}
					else 
					{
						lcs[i,j] = lcs[i-1,j-1];
						backTracer [i,j] = BackTracking.NEITHER;
					}

					if( lcs[i-1,j] >= lcs[i,j] ) 
					{	
						lcs[i,j] = lcs[i-1,j];
						backTracer[i,j] = BackTracking.UP;
						w[i,j] = 0;
					}

					if( lcs[i,j-1] >= lcs[i,j] ) 
					{
						lcs[i,j] = lcs[i,j-1];
						backTracer [i,j] = BackTracking.LEFT;
						w[i,j] = 0;
					}
				}
			}
			
			i=m; 
			j=n;
			
			string subseq="";
			int p=lcs[i,j];

			//trace the backtracking matrix.
			while( i > 0 || j > 0 ) 
			{
				if( backTracer[i,j] == BackTracking.UP_AND_LEFT ) 
				{
					i--;
					j--;
					subseq = list1[i] + subseq;
					Trace.WriteLine(i + " " + list1[i] + " " + j) ;
				}
	
				else if( backTracer[i,j] == BackTracking.UP ) 
				{
					i--;
				}
	
				else if( backTracer[i,j] == BackTracking.LEFT ) 
				{
					j--;
				}
			}
			
			
			return subseq ;
		}

	}
}
