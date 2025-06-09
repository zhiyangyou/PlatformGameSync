namespace BEPU_Adapter.PureLogic.Base.Layers {
    public class BEPU_LayerMatrix {
        private bool[,] matrix = null;

        public BEPU_LayerMatrix() {
            var layerCount = (int)BEPU_LayerDefaine.LayerCount;
            matrix = new bool[layerCount, layerCount];
            for (int i = 0; i < layerCount; i++) {
                for (int j = 0; j < layerCount; j++) {
                    matrix[i, j] = true;
                }
            }
        }

        public void Set(BEPU_LayerDefaine layerA, BEPU_LayerDefaine layerB, bool value) {
            matrix[(int)layerA, (int)layerB] = value;
            matrix[(int)layerB, (int)layerA] = value;
        }
    }
}