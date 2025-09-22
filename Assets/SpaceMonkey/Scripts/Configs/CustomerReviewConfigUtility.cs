using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SpaceMonkey.Scripts.Configs
{
    public static class CustomerReviewConfigUtility
    {
        [MenuItem("Tools/CustomerReview/Fix Reviews")]
        public static void FixReviews()
        {
            // Build dictionary of review messages -> correct type
            var mapping = new List<(string, ReviewType)>
            {
                // Extreme Low Material
                (
                    "It tasted fine, but the overall texture felt cheap, like they skimped on the ingredients.",
                    ReviewType.ExtremeLowMaterial
                ),
                (
                    "I was hoping for something more substantial, but the ingredients used clearly weren't the best, leaving a disappointing aftertaste.",
                    ReviewType.ExtremeLowMaterial
                ),
                (
                    "The flavor was acceptable, but the components felt low-quality, and it just didn't have that 'premium' feel.",
                    ReviewType.ExtremeLowMaterial
                ),
                // Extreme High Material
                (
                    "You could really taste the quality of the ingredients; everything felt fresh and high-grade.",
                    ReviewType.ExtremeHighMaterial
                ),
                (
                    "I was impressed by the richness and depth of flavor, a testament to the excellent ingredients they used.",
                    ReviewType.ExtremeHighMaterial
                ),
                (
                    "The texture was just perfect, clearly made with top-notch materials that made a real difference.",
                    ReviewType.ExtremeHighMaterial
                ),
                // Extreme Low Packaging
                (
                    "The packaging felt flimsy and cheap, and the item arrived slightly damaged. It didn't give me a sense of quality.",
                    ReviewType.ExtremeLowPackaging
                ),
                (
                    "While the product was okay, the packaging was incredibly wasteful and difficult to open, which was frustrating.",
                    ReviewType.ExtremeLowPackaging
                ),
                (
                    "The labeling was blurry and hard to read, and the overall presentation felt very unprofessional.",
                    ReviewType.ExtremeLowPackaging
                ),

                // Extreme High Packaging
                (
                    "The packaging was beautiful and practical, keeping the product fresh and looking great.",
                    ReviewType.ExtremeHighPackaging
                ),
                (
                    "I loved the attention to detail in the packaging; it was sturdy, elegant, and clearly designed with care.",
                    ReviewType.ExtremeHighPackaging
                ),
                (
                    "Attractive and also environmentally friendly packaging!  Much appreciated.",
                    ReviewType.ExtremeHighPackaging
                ),
                // Extreme High Price
                (
                    "For the quality received, the price was simply too high. I felt like I overpaid significantly.",
                    ReviewType.ExtremeHighPrice
                ),
                (
                    "It was an okay product, but the price point didn't match the value. It felt overpriced compared to similar items."
                    ,
                    ReviewType.ExtremeHighPrice
                ),
                (
                    "I was expecting more for the cost. The price didn't justify the overall experience.",
                    ReviewType.ExtremeHighPrice
                ),
// Extreme Low Price
                (
                    "The price was very reasonable. I felt like I got excellent value for my money.",
                    ReviewType.ExtremeLowPrice
                ),
                (
                    "The price was surprisingly fair. I might buy even more next time!", ReviewType.ExtremeLowPrice),
                (
                    "I was happy to pay such a low price. Keep it up!", ReviewType.ExtremeLowPrice),

// Extreme Slipshod TTP
                (
                    "It tasted rushed, like minimal effort was put into it. I expected more attention to detail.",
                    ReviewType.ExtremeSlipshodTTP
                ),
                (
                    "It lacked the finesse I associate with handcrafted goods. It felt mass-produced and impersonal.",
                    ReviewType.ExtremeSlipshodTTP
                ),
                (
                    "The presentation was sloppy, and it just didn't feel like a product made with care.",
                    ReviewType.ExtremeSlipshodTTP
                ),

// Extreme Diligent TTP
                (
                    "You could tell a lot of time and care went into making this. It tasted like it was made with passion.",
                    ReviewType.ExtremeDiligentTTP
                ),
                (
                    "The intricate details and perfect texture showed a real dedication to the craft. It was evident they took their time.",
                    ReviewType.ExtremeDiligentTTP
                ),
                (
                    "It felt like a labor of love. The quality and flavor were a testament to the effort put into it.",
                    ReviewType.ExtremeDiligentTTP
                ),

// Big Change Material / TTP
                (
                    "The increase in quality is truly impressive. Keep up the fine work, [company name]!",
                    ReviewType.BigChangeMaterial
                ),
                (
                    "The increase in quality is truly impressive. Keep up the fine work, [company name]!",
                    ReviewType.BigChangeTTP
                ),
                (
                    "I wasn't crazy about this product the first time, but it's just about perfect now – clearly, they haven't compromised on anything."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "I wasn't crazy about this product the first time, but it's just about perfect now – clearly, they haven't compromised on anything."
                    ,
                    ReviewType.BigChangeTTP
                ),
                (
                    "My previous experience with [product name] was less than stellar. I decided to give it another chance and BOY am I glad I did! So much better this time around."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "My previous experience with [product name] was less than stellar. I decided to give it another chance and BOY am I glad I did! So much better this time around."
                    ,
                    ReviewType.BigChangeTTP
                ),
                (
                    "I wasn't overly impressed with my previous purchase, but this time it was absolutely delicious; they've clearly made some improvements."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "I wasn't overly impressed with my previous purchase, but this time it was absolutely delicious; they've clearly made some improvements."
                    ,
                    ReviewType.BigChangeTTP
                ),
                (
                    "I was hesitant to try it again after my initial experience, but I'm so glad I did; it was a complete turnaround.",
                    ReviewType.BigChangeMaterial
                ),
                (
                    "I was hesitant to try it again after my initial experience, but I'm so glad I did; it was a complete turnaround.",
                    ReviewType.BigChangeTTP
                ),
                (
                    "My last impression wasn't great, but this recent purchase of [product name] has completely changed my mind; I'm now a fan."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "My last impression wasn't great, but this recent purchase of [product name] has completely changed my mind; I'm now a fan."
                    ,
                    ReviewType.BigChangeTTP
                ),
                (
                    "[product name] was fantastic last time, but this recent purchase just didn't taste the same; I'm not sure what changed."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "[product name] was fantastic last time, but this recent purchase just didn't taste the same; I'm not sure what changed."
                    ,
                    ReviewType.BigChangeTTP
                ),
                (
                    "I raved about [product name] after my last order, but this time, it was noticeably different and unfortunately, not in a good way."
                    ,
                    ReviewType.BigChangeMaterial
                ),
                (
                    "I raved about [product name] after my last order, but this time, it was noticeably different and unfortunately, not in a good way."
                    ,
                    ReviewType.BigChangeTTP
                ), // Big Change Packaging
                (
                    "I was so excited when I received my [product name] again after how much I enjoyed it before, but this batch was a real letdown."
                    ,
                    ReviewType.BigChangePackaging
                ),
            };
            var aaa = new List<ReviewInfo>();
            foreach (var pair in mapping)
            {
                aaa.Add(new ReviewInfo(pair.Item2, pair.Item1, false));
            }


            CustomerReviewConfig asset = ScriptableObject.CreateInstance<CustomerReviewConfig>();
            asset.Set(aaa.ToArray());
            AssetDatabase.CreateAsset(asset, "Assets/SpaceMonkey/Resources/Configs/CustomerReviewConfig.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("CustomerReviewConfig fixed!");
        }

        private static void Create(ReviewInfo[] a)
        {
        }
    }
}