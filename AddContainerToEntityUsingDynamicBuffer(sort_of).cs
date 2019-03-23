    // Since you can't actually add a Native Container (Such as NativeList, NativeHashMap, NativeMultiHashMap, etc) into an IComponentData, this is the next best thing. 
    // The below is how you create a Dynamic Buffer (DynamicBuffer which would be a struct derived from IBufferElementData) to an Entity that can act as an Array. It can hold anything that is blittable I believe. 
    //Technically the dynamic buffer itself only holds a single value type but becomes an array because it essentially stacks (at least that is how I choose to believe that it works, lol).

    // This can just be its own new file, such as MyDataBufferComponent.cs or if you wanted you can add this code to another already created component
    
    // This limits the number of actual items that the buffer will contain. Keep it as low as you can while making sure you have enough,
    // it can grow but not get resized again unless you get rid of the one on the entity and create/add a new one
    [InternalBufferCapacity(3)]
    public struct MyDataBuffer : IBufferElementData
    {
        // This can be pretty much whatever, such as int, float, or a custom struct you have created which can contain additional data fields, as long as that struct is blittable and contains no containers
        public MyData mydata; 
    }
    
    // --------------------------------------------------
    
    // Creates a new list of the type of your buffer (doesn't have to be a list, can be a single item)
    public List<MyDataBuffer> dataBufferList = new List<MyDataBuffer>(); 

    // Just example, this would usually instead be in a ComponentSystem, or if its IJobProcessComponentDataWithEntity the Entity would be there already
    void AddBufferItems(Entity entity, MyData myDataForBuffer) 
    {
        // Get access to the EntityManager so you can work with Entities
        manager = World.Active.GetOrCreateManager<EntityManager>();
       
        // Adds the actual buffer component to your entity
        manager.AddBuffer<MyDataBuffer>(entity); 
        
        // Gets the buffer you just added to your Entity so you can then add the data to it
        var myBuffer = manager.GetBuffer<MyDataBuffer>(entity); 
 
        // This is just whatever data you are going to actually add into the buffer/array attached to the entity
        var myData = myDataForBuffer; 
        
        // This creates a new buffer object and passes the actual data you want to have on the entity into the new buffer object and adds it to the list we created
        dataBufferList.Add(new MyDataBuffer(myData)); 
 
        // Since we made a list of the things we wanted to add, we just iterate over that list and add them to the buffer
        for (int i = 0; i < dataBufferList.Count; i++) 
        {
            // Adds each element of the list we created that contains the buffer objects which hold the data we want to add, to the actual buffer in which we used GetBuffer to gain access via the manager
            myBuffer.Add(dataBufferList[i]); 
        }
    }
