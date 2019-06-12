using Microsoft.Xna.Framework;
using Pathoschild.Stardew.Common.Utilities;
using StardewModdingAPI;
using StardewValley;
using StardewValley.TerrainFeatures;
using SObject = StardewValley.Object;

namespace Pathoschild.Stardew.TractorMod.Framework.Attachments
{
    /// <summary>An attachment for any configured custom tools.</summary>
    internal class CustomAttachment : BaseAttachment
    {
        /*********
        ** Fields
        *********/
        /// <summary>The enabled custom tool or item names.</summary>
        private readonly InvariantHashSet CustomNames;


        /*********
        ** Public methods
        *********/
        /// <summary>Construct an instance.</summary>
        /// <param name="customAttachments">The enabled custom tool or item names.</param>
        /// <param name="reflection">Simplifies access to private code.</param>
        public CustomAttachment(string[] customAttachments, IReflectionHelper reflection)
            : base(reflection)
        {
            this.CustomNames = new InvariantHashSet(customAttachments);
        }

        /// <summary>Get whether the tool is currently enabled.</summary>
        /// <param name="player">The current player.</param>
        /// <param name="tool">The tool selected by the player (if any).</param>
        /// <param name="item">The item selected by the player (if any).</param>
        /// <param name="location">The current location.</param>
        public override bool IsEnabled(Farmer player, Tool tool, Item item, GameLocation location)
        {
            return
                (tool != null && this.CustomNames.Contains(tool.Name))
                || (item != null && this.CustomNames.Contains(item.Name));
        }

        /// <summary>Apply the tool to the given tile.</summary>
        /// <param name="tile">The tile to modify.</param>
        /// <param name="tileObj">The object on the tile.</param>
        /// <param name="tileFeature">The feature on the tile.</param>
        /// <param name="player">The current player.</param>
        /// <param name="tool">The tool selected by the player (if any).</param>
        /// <param name="item">The item selected by the player (if any).</param>
        /// <param name="location">The current location.</param>
        public override bool Apply(Vector2 tile, SObject tileObj, TerrainFeature tileFeature, Farmer player, Tool tool, Item item, GameLocation location)
        {
            // apply tool
            if (tool != null && this.CustomNames.Contains(tool.Name))
                return this.UseToolOnTile(tool, tile);

            // apply item
            if (item != null && item.Stack > 0 && this.CustomNames.Contains(item.Name))
            {
                if (item is SObject obj && obj.canBePlacedHere(location, tile) && obj.placementAction(location, (int)(tile.X * Game1.tileSize), (int)(tile.Y * Game1.tileSize), player))
                {
                    this.ConsumeItem(player, item);
                    return true;
                }
            }

            return false;
        }
    }
}