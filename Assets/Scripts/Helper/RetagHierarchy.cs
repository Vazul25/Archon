using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Helper
{
    public static class RetagHierarchy
    {
        public static void Retag(Transform trans, string tag) {
            trans.gameObject.tag = tag;
            foreach (Transform t in trans)
            {
                Retag(t, tag);
            }
        }
    }
}
