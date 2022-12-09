using System.Text.RegularExpressions;

namespace AMS2_core.Global
{
    public class Constants
    {
        public string toolName = "ASSIGN MANAGER SYSTEM";
        public string toolDescription = "UPDATE THE DATABASE";
        public string toolVersion = "v.8.1.2";

        public string jpgpattern = "*.jpg";
        public string psdpattern = "*.psd";
        public string psbPattern = "*.psb";

        public string MWEQ = "MWEQ";
        public string MWEQ20 = "MWEQ20";
        public string MA = "MA";
        public string MA20 = "MA20";
        public string PS = "PS";
        public string PS20 = "PS20";
        public string YA = "YA";
        public string YA20 = "YA20";
        public string YAPS = "YAPS";
        public string YAPS20 = "YAPS20";
        public string FTW = "FTW";

        public string root = @"N:\";
        public string temp = @"K:\TEMP PSDs"; // temp

        public string pathRootMWEQ = @"N:\Garments"; // the root folder for Men-Women-Equipment Garments
        public string pathRootMWEQ20 = @"N:\Garments\20 Tracksuits"; //Tracksuits

        public string pathRootMA = @"N:\Garments MA"; // the root folder to Maternity Garments
        public string pathRootMA20 = @"N:\Garments MA\20_Tracksuits"; //Tracksuits

        public string pathRootPlusSize = @"N:\Garments PS"; // the root folder for PlusSize Garments
        public string pathRootPlusSize20 = @"N:\Garments PS\20 Tracksuits"; //Tracksuits


        public string pathRootYA = @"N:\Garments YA"; // the root folder for Young Athletes Garments
        public string pathRootYA20 = @"N:\Garments YA\20 Tracksuits"; //Tracksuits

        public string pathRootYAPS = @"N:\Garments YAPS"; //the root folder for YAPS
        public string pathRootYAPS20 = @"N:\Garments YAPS\20_Tracksuits"; //Tracksuits

        public string pathRootFTW = @"N:\Footwear"; // the root folder for Footwear garments  

        public string geometryToSheetPath = @"M:\Z_Software Assets\3ds Max\BorakaScriptPack_vol1\assignmanager\AssignManagerSystem\";

        //public string fileRegex = new Regex(@"\bT_(?<garment>[N|S]\d{2}[A-Z]\d{3})_(?<season>[A-Z][A-Z]\d{2})_(?<category>[A-Z][A-Z])_(?<sku>.{6})-.{3}_D\b").ToString();

        //updated regex
        public string fileRegex = new Regex(@"\bT_(?<garment>[N|S]\d{2}[A-Z]\d{3})_(?<season>[A-Z][A-Z]\d{2})_(?<category>[A-Z][A-Z])_(?<sku>.{6})[-|_](?<color>.{3})_D\b").ToString();


    }
}
