using OtherSideCore.Infrastructure;

namespace OtherSideCore.Infrastructure.Tests.Utils
{
   public class UtilsTests
   {
      [Fact]
      public void GetMaxSearchDistance_ReturnCorrectDistance()
      {
         Assert.Equal(0, Infrastructure.Utils.GetMaxSearchDistance(null));
         Assert.Equal(0, Infrastructure.Utils.GetMaxSearchDistance(""));
         Assert.Equal(1, Infrastructure.Utils.GetMaxSearchDistance("a"));
         Assert.Equal(2, Infrastructure.Utils.GetMaxSearchDistance("test"));
         Assert.Equal(2, Infrastructure.Utils.GetMaxSearchDistance("testt"));
         Assert.Equal(7, Infrastructure.Utils.GetMaxSearchDistance("test test test"));
      }

      [Fact]
      public void LevenshteinDistance_ReturnCorrectDistance()
      {
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance(null, null));
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance(null, ""));
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance("", null));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance(null, "aaa"));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aaa", null));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aaa", "bbb"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("aab", "bbb"));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aab", "bbba"));
         Assert.Equal(1, Infrastructure.Utils.LevenshteinDistance("test", "tes"));
         Assert.Equal(1, Infrastructure.Utils.LevenshteinDistance("pppqrst", "ppqrst"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("apres", "aprè"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("tèt", "test"));
      }

      [Fact]
      public void EditDistance_ReturnCorrectDistance()
      {
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance(null, null));
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance(null, ""));
         Assert.Equal(0, Infrastructure.Utils.LevenshteinDistance("", null));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance(null, "aaa"));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aaa", null));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aaa", "bbb"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("aab", "bbb"));
         Assert.Equal(3, Infrastructure.Utils.LevenshteinDistance("aab", "bbba"));
         Assert.Equal(1, Infrastructure.Utils.LevenshteinDistance("test", "tes"));
         Assert.Equal(1, Infrastructure.Utils.LevenshteinDistance("pppqrst", "ppqrst"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("apres", "aprè"));
         Assert.Equal(2, Infrastructure.Utils.LevenshteinDistance("tèt", "test"));
      }
   }
}
