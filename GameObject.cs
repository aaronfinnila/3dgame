using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3dgame;

public class GameObject {
    public Model Model { get; set; }
    public Vector3 Position { get; set; }
    public bool IsActive { get; set; }
    public BoundingSphere BoundingSphere { get; set; }

    public GameObject() {
        Model = null;
        Position = Vector3.Zero;
        IsActive = false;
        BoundingSphere = new BoundingSphere();
    }

    public BoundingSphere CalculateBoundingSphere() {
        BoundingSphere mergedSphere = new BoundingSphere();
        BoundingSphere[] boundingSpheres;
        int index = 0;
        int meshCount = Model.Meshes.Count;

        boundingSpheres = new BoundingSphere[meshCount];
        foreach (ModelMesh mesh in Model.Meshes) {
            boundingSpheres[index++] = mesh.BoundingSphere;
        }

        mergedSphere = boundingSpheres[0];
        if ((Model.Meshes.Count) > 1) {
            index = 1;
            do {
                mergedSphere = BoundingSphere.CreateMerged(mergedSphere, boundingSpheres[index]);
                index++;
            } while (index < Model.Meshes.Count);
        }
        mergedSphere.Center.Y = mergedSphere.Radius / 2;
        return mergedSphere;
    }

    internal void DrawBoundingSphere(Matrix view, Matrix projection, GameObject boundingSphereModel) {
        Matrix scaleMatrix = Matrix.CreateScale(BoundingSphere.Radius);
        Matrix translateMatrix = Matrix.CreateTranslation(BoundingSphere.Center);
        Matrix worldMatrix = scaleMatrix * translateMatrix;

        foreach (ModelMesh mesh in boundingSphereModel.Model.Meshes)
        {
            foreach (BasicEffect effect in mesh.Effects)
            {
                effect.World = worldMatrix;
                effect.View = view;
                effect.Projection = projection;
            }
            mesh.Draw();
        }
    }
}