namespace Scripts.Items {
    ///<summary>The base class for all accessory affixes</summary>
    public class AccessoryAffix {
        public string Name { get; set; }
        ///<summary>This affix's tier. Tiers start at 1.
        public byte Tier { get; set; }
        ///<summary>The mechanic attatch to this affix, ie, X% damage conversion, X% damage reduction, etc.</summary>
        public int EffectID { get; set; }
        ///<summary>How much this affix effects the value of the accessory.</summary>
        public float ValueModifier { get; set; }
        //TODO Add full stat modifications
    }
}
