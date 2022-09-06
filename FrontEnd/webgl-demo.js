var cubeRotationX = 0.0;
var cubeRotationY = 0.0;
var lastMousePositionX = null
var lastMousePositionY = null
var vertexCount = 0

main();

//
// Start here
//
async function main() {
  const wsUri = "ws://" + location.hostname + ":3000/echo";
  const websocket = await new WebSocket(wsUri);

  console.log(location.hostname)

  websocket.onopen = () => websocket.send("Hello this works!");

  websocket.onmessage = (e) => {
    console.log('Message received')
    console.log(e.data)
  };

  const canvas = document.querySelector("#glcanvas");
  const gl =
    canvas.getContext("webgl") || canvas.getContext("experimental-webgl");

    var mousedown = false;
    canvas.onmousedown = function(ev){    
      mousedown = true;  
    };

    canvas.onmouseup = function(ev){    
      mousedown = false
      lastMousePositionX = null
      lastMousePositionY = null
    };

    canvas.addEventListener("mousemove", function(){onMouseMove(event)});

    function onMouseMove(ev){
      if (mousedown == false)
        return

      var mousePositionX = (2*ev.clientX/canvas.width) - 1;
      var mousePositionY = (2*ev.clientY/(canvas.height*(-1))) + 1;

      if (lastMousePositionX == null)
      {
        lastMousePositionX = mousePositionX
      }
      else
      {
        let cubeDiffY = 5 * (mousePositionX - lastMousePositionX)
        if (Math.abs(cubeRotationX) > (Math.PI / 2))
          cubeRotationY -= cubeDiffY
        else
          cubeRotationY += cubeDiffY
        lastMousePositionX = mousePositionX
      }

      if (lastMousePositionY == null)
      {
        lastMousePositionY = mousePositionY
      }
      else
      {
        let cubeDiffX = 5 * (mousePositionY - lastMousePositionY)
        cubeRotationX += cubeDiffX
        lastMousePositionY = mousePositionY
      }

      if (cubeRotationX > (2 * Math.PI))
        cubeRotationX = 0
      if (cubeRotationX < (-2 * Math.PI))
        cubeRotationX = 0

      if (cubeRotationY > (2 * Math.PI))
        cubeRotationY = 0
      if (cubeRotationY < (-2 * Math.PI))
        cubeRotationY = 0

      console.log(cubeRotationX)
      console.log(cubeRotationY)

      websocket.send(cubeRotationX)
      websocket.send(cubeRotationY)
    }

  // If we don't have a GL context, give up now

  if (!gl) {
    alert(
      "Unable to initialize WebGL. Your browser or machine may not support it."
    );
    return;
  }

  // Vertex shader program

  const vsSource = `
    attribute vec4 aVertexPosition;
    attribute vec4 aVertexColor;
    attribute vec2 aTextureCoord;

    uniform mat4 uModelViewMatrix;
    uniform mat4 uProjectionMatrix;

    varying highp vec2 vTextureCoord;
    varying lowp vec4 vColor;

    void main(void) {
      gl_Position = uProjectionMatrix * uModelViewMatrix * aVertexPosition;
      vTextureCoord = aTextureCoord;
      vColor = aVertexColor;
    }
  `;

  // Fragment shader program

  // const fsSource = `
  //   varying lowp vec4 vColor;

  //   void main(void) {
  //     gl_FragColor = vColor;
  //   }
  // `;

  const fsSource = `
    precision mediump float;
    varying lowp vec4 vColor;
    varying highp vec2 vTextureCoord;

    void main(void) {
      float maxX = 1.0 - 0.05;
      float minX = 0.05;
      float maxY = maxX;
      float minY = minX;

      if (vTextureCoord.x < maxX && vTextureCoord.x > minX &&
        vTextureCoord.y < maxY && vTextureCoord.y > minY) {
        gl_FragColor = vColor;
      } else {
        gl_FragColor = vec4(0, 0, 0, 1);
      }
    }
  `;

  // Initialize a shader program; this is where all the lighting
  // for the vertices and so forth is established.
  const shaderProgram = initShaderProgram(gl, vsSource, fsSource);

  // Collect all the info needed to use the shader program.
  // Look up which attributes our shader program is using
  // for aVertexPosition, aVertexColor and also
  // look up uniform locations.
  const programInfo = {
    program: shaderProgram,
    attribLocations: {
      vertexPosition: gl.getAttribLocation(shaderProgram, "aVertexPosition"),
      vertexColor: gl.getAttribLocation(shaderProgram, "aVertexColor"),
      textureCoord: gl.getAttribLocation(shaderProgram, "aTextureCoord"),
    },
    uniformLocations: {
      projectionMatrix: gl.getUniformLocation(
        shaderProgram,
        "uProjectionMatrix"
      ),
      modelViewMatrix: gl.getUniformLocation(shaderProgram, "uModelViewMatrix"),
    },
  };

  // Here's where we call the routine that builds all the
  // objects we'll be drawing.
  const buffers = await initBuffers(gl);

  var then = 0;

  // Draw the scene repeatedly
  function render(now) {
    now *= 0.0001; // convert to seconds
    deltaTime = now - then;
    then = now;

    deltaTime = 0.01

    drawScene(gl, programInfo, buffers, deltaTime);

    requestAnimationFrame(render);
  }
  requestAnimationFrame(render);
}

//
// initBuffers
//
// Initialize the buffers we'll need. For this demo, we just
// have one object -- a simple three-dimensional cube.
//
async function initBuffers(gl) {
  // Create a buffer for the cube's vertex positions.

  const positionBuffer = gl.createBuffer();

  // Select the positionBuffer as the one to apply buffer
  // operations to from here out.

  gl.bindBuffer(gl.ARRAY_BUFFER, positionBuffer);

  // Now create an array of positions for the cube.

  let cubeData = await fetch("/cube.json")
  .then(response => {
    return response.json();
  })

  vertexCount = cubeData.VertexCount
  console.log(cubeData)

  // Now pass the list of positions into WebGL to build the
  // shape. We do this by creating a Float32Array from the
  // JavaScript array, then use it to fill the current buffer.

  gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(cubeData.Positions), gl.STATIC_DRAW);

  // Now set up the colors for the faces. We'll use solid colors
  // for each face.

  const faceColors = [
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white
    [1.0, 1.0, 1.0, 1.0], // Front face: white

    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red
    [1.0, 0.0, 0.0, 1.0], // Back face: red

    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green
    [0.0, 0.8, 0.0, 1.0], // Top face: green

    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue
    [0.0, 0.0, 1.0, 1.0], // Bottom face: blue

    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow
    [1.0, 1.0, 0.0, 1.0], // Right face: yellow

    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
    [1.0, 0.5, 0.0, 1.0], // Left face: orange
  ];

  // Convert the array of colors into a table for all the vertices.

  var colors = [];

  for (var j = 0; j < faceColors.length; ++j) {
    const c = faceColors[j];

    // Repeat each color four times for the four vertices of the face
    colors = colors.concat(c, c, c, c);
  }

  const colorBuffer = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER, colorBuffer);
  gl.bufferData(gl.ARRAY_BUFFER, new Float32Array(colors), gl.STATIC_DRAW);

  // Now set up the texture coordinates for the faces.

  const textureCoordBuffer = gl.createBuffer();
  gl.bindBuffer(gl.ARRAY_BUFFER, textureCoordBuffer);

  gl.bufferData(
    gl.ARRAY_BUFFER,
    new Float32Array(cubeData.TextureCoords),
    gl.STATIC_DRAW
  );

  // Build the element array buffer; this specifies the indices
  // into the vertex arrays for each face's vertices.

  const indexBuffer = gl.createBuffer();
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, indexBuffer);

  // This array defines each face as two triangles, using the
  // indices into the vertex array to specify each triangle's
  // position.

  // Now send the element array to GL

  gl.bufferData(
    gl.ELEMENT_ARRAY_BUFFER,
    new Uint16Array(cubeData.Indices),
    gl.STATIC_DRAW
  );

  return {
    position: positionBuffer,
    color: colorBuffer,
    indices: indexBuffer,
    textureCoord: textureCoordBuffer,
  };
}

//
// Draw the scene.
//
function drawScene(gl, programInfo, buffers, deltaTime) {
  gl.clearColor(0.4, 0.4, 0.5, 1.0); // Clear to black, fully opaque
  gl.clearDepth(1.0); // Clear everything
  gl.enable(gl.DEPTH_TEST); // Enable depth testing
  gl.depthFunc(gl.LEQUAL); // Near things obscure far things

  // Clear the canvas before we start drawing on it.

  gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

  // Create a perspective matrix, a special matrix that is
  // used to simulate the distortion of perspective in a camera.
  // Our field of view is 45 degrees, with a width/height
  // ratio that matches the display size of the canvas
  // and we only want to see objects between 0.1 units
  // and 100 units away from the camera.

  const fieldOfView = (45 * Math.PI) / 180; // in radians
  const aspect = gl.canvas.clientWidth / gl.canvas.clientHeight;
  const zNear = 0.1;
  const zFar = 100.0;
  const projectionMatrix = mat4.create();

  // note: glmatrix.js always has the first argument
  // as the destination to receive the result.
  mat4.perspective(projectionMatrix, fieldOfView, aspect, zNear, zFar);

  // Set the drawing position to the "identity" point, which is
  // the center of the scene.
  const modelViewMatrix = mat4.create();

  // Now move the drawing position a bit to where we want to
  // start drawing the square.
  mat4.translate(
    modelViewMatrix, // destination matrix
    modelViewMatrix, // matrix to translate
    [-0.0, 0.0, -20.0]
  ); // amount to translate

  // let quart = [xComp * Math.sin(angle / 2), yComp * Math.sin(angle / 2), 0, Math.cos(angle / 2)]
  // let mat = mat4.create()
  // mat4.fromQuat(mat, quart)
  // if (lastMat == undefined)
  //   lastMat = mat
  // else
  //   mat4.multiply(lastMat, lastMat, mat)

  // mat4.multiply(modelViewMatrix, modelViewMatrix, lastMat)
  
  mat4.rotate(
    modelViewMatrix, // destination matrix
    modelViewMatrix, // matrix to rotate
    -cubeRotationX , // amount to rotate in radians
    [1, 0, 0]
  ); // axis to rotate around (X)

  mat4.rotate(
    modelViewMatrix, // destination matrix
    modelViewMatrix, // matrix to rotate
    cubeRotationY,// * Math.cos(cubeRotationX), // amount to rotate in radians
    [0, 1, 0]
  ); // axis to rotate around (Y)

  // mat4.rotate(
  //   modelViewMatrix, // destination matrix
  //   modelViewMatrix, // matrix to rotate
  //   cubeRotationX * Math.sin(cubeRotationX), // amount to rotate in radians
  //   [0, 0, 1]
  // ); // axis to rotate around (Z)
  
  

  // Y rotation
  // let Yquat = [0, Math.sin(cubeRotationY / 2), 0, Math.cos(cubeRotationY / 2)]
  // let matY = mat4.create()
  // mat4.fromQuat(matY, Yquat)
  // mat4.multiply(modelViewMatrix, modelViewMatrix, matY)

  // // Z rotation
  // let Zquat = [0, 0, Math.sin((cubeRotationX * Math.sin(cubeRotationY)) / 2), Math.cos((cubeRotationX * Math.sin(cubeRotationY)) / 2)]
  // let matZ = mat4.create()
  // mat4.fromQuat(matZ, Zquat)
  // mat4.multiply(modelViewMatrix, modelViewMatrix, matZ)

  // // X rotation
  // let Xquat = [Math.sin((cubeRotationX * Math.cos(cubeRotationY)) / 2), 0, 0, Math.cos((cubeRotationX * Math.cos(cubeRotationY)) / 2)]
  // let matX = mat4.create()
  // mat4.fromQuat(matX, Xquat)
  // mat4.multiply(modelViewMatrix, modelViewMatrix, matX)

  // Tell WebGL how to pull out the positions from the position
  // buffer into the vertexPosition attribute
  {
    const numComponents = 3;
    const type = gl.FLOAT;
    const normalize = false;
    const stride = 0;
    const offset = 0;
    gl.bindBuffer(gl.ARRAY_BUFFER, buffers.position);
    gl.vertexAttribPointer(
      programInfo.attribLocations.vertexPosition,
      numComponents,
      type,
      normalize,
      stride,
      offset
    );
    gl.enableVertexAttribArray(programInfo.attribLocations.vertexPosition);
  }

  // Tell WebGL how to pull out the colors from the color buffer
  // into the vertexColor attribute.
  {
    const numComponents = 4;
    const type = gl.FLOAT;
    const normalize = false;
    const stride = 0;
    const offset = 0;
    gl.bindBuffer(gl.ARRAY_BUFFER, buffers.color);
    gl.vertexAttribPointer(
      programInfo.attribLocations.vertexColor,
      numComponents,
      type,
      normalize,
      stride,
      offset
    );
    gl.enableVertexAttribArray(programInfo.attribLocations.vertexColor);
  }

  // Tell WebGL how to pull out the texture coordinates from
  // the texture coordinate buffer into the textureCoord attribute.
  {
    const numComponents = 2;
    const type = gl.FLOAT;
    const normalize = false;
    const stride = 0;
    const offset = 0;
    gl.bindBuffer(gl.ARRAY_BUFFER, buffers.textureCoord);
    gl.vertexAttribPointer(
      programInfo.attribLocations.textureCoord,
      numComponents,
      type,
      normalize,
      stride,
      offset
    );
    gl.enableVertexAttribArray(programInfo.attribLocations.textureCoord);
  }

  // Tell WebGL which indices to use to index the vertices
  gl.bindBuffer(gl.ELEMENT_ARRAY_BUFFER, buffers.indices);

  // Tell WebGL to use our program when drawing

  gl.useProgram(programInfo.program);

  // Set the shader uniforms

  gl.uniformMatrix4fv(
    programInfo.uniformLocations.projectionMatrix,
    false,
    projectionMatrix
  );
  gl.uniformMatrix4fv(
    programInfo.uniformLocations.modelViewMatrix,
    false,
    modelViewMatrix
  );

  {
    const type = gl.UNSIGNED_SHORT;
    const offset = 0;
    gl.drawElements(gl.TRIANGLES, vertexCount, type, offset);
  }

  // Update the rotation for the next draw

  //cubeRotation += deltaTime;
}

//
// Initialize a shader program, so WebGL knows how to draw our data
//
function initShaderProgram(gl, vsSource, fsSource) {
  const vertexShader = loadShader(gl, gl.VERTEX_SHADER, vsSource);
  const fragmentShader = loadShader(gl, gl.FRAGMENT_SHADER, fsSource);

  // Create the shader program

  const shaderProgram = gl.createProgram();
  gl.attachShader(shaderProgram, vertexShader);
  gl.attachShader(shaderProgram, fragmentShader);
  gl.linkProgram(shaderProgram);

  // If creating the shader program failed, alert

  if (!gl.getProgramParameter(shaderProgram, gl.LINK_STATUS)) {
    alert(
      "Unable to initialize the shader program: " +
        gl.getProgramInfoLog(shaderProgram)
    );
    return null;
  }

  return shaderProgram;
}

//
// creates a shader of the given type, uploads the source and
// compiles it.
//
function loadShader(gl, type, source) {
  const shader = gl.createShader(type);

  // Send the source to the shader object

  gl.shaderSource(shader, source);

  // Compile the shader program

  gl.compileShader(shader);

  // See if it compiled successfully

  if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
    alert(
      "An error occurred compiling the shaders: " + gl.getShaderInfoLog(shader)
    );
    gl.deleteShader(shader);
    return null;
  }

  return shader;
}
