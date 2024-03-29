﻿using System.Collections;

namespace WinCachebox
{
  internal class HtmlEntities
  {
    private static string[] _entitiesList = new string[] { 
        "\"-quot", "&-amp", "<-lt", ">-gt", "\x00a0-nbsp", "\x00a1-iexcl", "\x00a2-cent", "\x00a3-pound", "\x00a4-curren", "\x00a5-yen", "\x00a6-brvbar", "\x00a7-sect", "\x00a8-uml", "\x00a9-copy", "\x00aa-ordf", "\x00ab-laquo", 
        "\x00ac-not", "\x00ad-shy", "\x00ae-reg", "\x00af-macr", "\x00b0-deg", "\x00b1-plusmn", "\x00b2-sup2", "\x00b3-sup3", "\x00b4-acute", "\x00b5-micro", "\x00b6-para", "\x00b7-middot", "\x00b8-cedil", "\x00b9-sup1", "\x00ba-ordm", "\x00bb-raquo", 
        "\x00bc-frac14", "\x00bd-frac12", "\x00be-frac34", "\x00bf-iquest", "\x00c0-Agrave", "\x00c1-Aacute", "\x00c2-Acirc", "\x00c3-Atilde", "\x00c4-Auml", "\x00c5-Aring", "\x00c6-AElig", "\x00c7-Ccedil", "\x00c8-Egrave", "\x00c9-Eacute", "\x00ca-Ecirc", "\x00cb-Euml", 
        "\x00cc-Igrave", "\x00cd-Iacute", "\x00ce-Icirc", "\x00cf-Iuml", "\x00d0-ETH", "\x00d1-Ntilde", "\x00d2-Ograve", "\x00d3-Oacute", "\x00d4-Ocirc", "\x00d5-Otilde", "\x00d6-Ouml", "\x00d7-times", "\x00d8-Oslash", "\x00d9-Ugrave", "\x00da-Uacute", "\x00db-Ucirc", 
        "\x00dc-Uuml", "\x00dd-Yacute", "\x00de-THORN", "\x00df-szlig", "\x00e0-agrave", "\x00e1-aacute", "\x00e2-acirc", "\x00e3-atilde", "\x00e4-auml", "\x00e5-aring", "\x00e6-aelig", "\x00e7-ccedil", "\x00e8-egrave", "\x00e9-eacute", "\x00ea-ecirc", "\x00eb-euml", 
        "\x00ec-igrave", "\x00ed-iacute", "\x00ee-icirc", "\x00ef-iuml", "\x00f0-eth", "\x00f1-ntilde", "\x00f2-ograve", "\x00f3-oacute", "\x00f4-ocirc", "\x00f5-otilde", "\x00f6-ouml", "\x00f7-divide", "\x00f8-oslash", "\x00f9-ugrave", "\x00fa-uacute", "\x00fb-ucirc", 
        "\x00fc-uuml", "\x00fd-yacute", "\x00fe-thorn", "\x00ff-yuml", "Œ-OElig", "œ-oelig", "Š-Scaron", "š-scaron", "Ÿ-Yuml", "ƒ-fnof", "ˆ-circ", "˜-tilde", "?-Alpha", "?-Beta", "G-Gamma", "?-Delta", 
        "?-Epsilon", "?-Zeta", "?-Eta", "T-Theta", "?-Iota", "?-Kappa", "?-Lambda", "?-Mu", "?-Nu", "?-Xi", "?-Omicron", "?-Pi", "?-Rho", "S-Sigma", "?-Tau", "?-Upsilon", 
        "F-Phi", "?-Chi", "?-Psi", "O-Omega", "a-alpha", "ß-beta", "?-gamma", "d-delta", "e-epsilon", "?-zeta", "?-eta", "?-theta", "?-iota", "?-kappa", "?-lambda", "µ-mu", 
        "?-nu", "?-xi", "?-omicron", "p-pi", "?-rho", "?-sigmaf", "s-sigma", "t-tau", "?-upsilon", "f-phi", "?-chi", "?-psi", "?-omega", "?-thetasym", "?-upsih", "?-piv", 
        " -ensp", " -emsp", "?-thinsp", "?-zwnj", "?-zwj", "?-lrm", "?-rlm", "–-ndash", "—-mdash", "‘-lsquo", "’-rsquo", "‚-sbquo", "“-ldquo", "”-rdquo", "„-bdquo", "†-dagger", 
        "‡-Dagger", "•-bull", "…-hellip", "‰-permil", "'-prime", "?-Prime", "‹-lsaquo", "›-rsaquo", "?-oline", "/-frasl", "€-euro", "I-image", "P-weierp", "R-real", "™-trade", "?-alefsym", 
        "?-larr", "?-uarr", "?-rarr", "?-darr", "?-harr", "?-crarr", "?-lArr", "?-uArr", "?-rArr", "?-dArr", "?-hArr", "?-forall", "?-part", "?-exist", "Ø-empty", "?-nabla", 
        "?-isin", "?-notin", "?-ni", "?-prod", "?-sum", "--minus", "*-lowast", "v-radic", "?-prop", "8-infin", "?-ang", "?-and", "?-or", "n-cap", "?-cup", "?-int", 
        "?-there4", "~-sim", "?-cong", "˜-asymp", "?-ne", "=-equiv", "=-le", "=-ge", "?-sub", "?-sup", "?-nsub", "?-sube", "?-supe", "?-oplus", "?-otimes", "?-perp", 
        "·-sdot", "?-lceil", "?-rceil", "?-lfloor", "?-rfloor", "<-lang", ">-rang", "?-loz", "?-spades", "?-clubs", "?-hearts", "?-diams", "'-apos"
     };
    private static Hashtable _entitiesLookupTable;
    private static object _lookupLockObject = new object();

    private HtmlEntities()
    {
    }

    internal static char Lookup(string entity)
    {
      if (_entitiesLookupTable == null)
      {
        lock (_lookupLockObject)
        {
          if (_entitiesLookupTable == null)
          {
            Hashtable hashtable = new Hashtable();
            foreach (string str in _entitiesList)
            {
              hashtable[str.Substring(2)] = str[0];
            }
            _entitiesLookupTable = hashtable;
          }
        }
      }
      object obj2 = _entitiesLookupTable[entity];
      if (obj2 != null)
      {
        return (char)obj2;
      }
      return '\0';
    }
  }
}

