import React, { useRef } from "react";

import "./App.css";
import ChartBar from "./components/CharBar";

const App = () => {
  const refChart = useRef(null);

  return (
    <div className="App">
      <header className="App-header">
        <span>Demo Chart</span>

        {/* <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.js</code> and save to reload.
        </p> */}
        {/* <DataTable
          dataKey={"id"}
          value={results}
          tableStyle={{ tableLayout: "fixed", borderColor: "white" }}
          editMode="cell"
          showGridlines
          onRowEditComplete={(e) => {
            console.log("compelete", e);
            setTimeout(() => {
              dispatch({ type: "update", data: { ...e } });
            }, 1000);
          }}
        >
          {columns.map((col) => (
            <Column {...col} key={`column-${col.field}`} />
          ))}
        </DataTable> */}
      </header>
      <button
        onClick={() => {
          if (refChart.current) {
            refChart.current.options.scales.x.min = 0;
            refChart.current.options.scales.x.max = 60;
            refChart.current.update();
          }
        }}
      >
        reset
      </button>
      <ChartBar ref={refChart} />
    </div>
  );
};

export default App;
