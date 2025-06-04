public static class DefaultAttr {
    public static  BEPU_PhysicMaterial _defaultMaterial = null;

    public static  BEPU_PhysicMaterial DefaultMaterial {
        get {
            if (_defaultMaterial == null) {
                _defaultMaterial = new BEPU_PhysicMaterial();
                _defaultMaterial.Bounciness = 0.3f;
                _defaultMaterial.KineticFriction = 1f;
                _defaultMaterial.StaticFriction = 1f;
            }
            return _defaultMaterial;
        }
    }
}