import React, { useReducer, useRef } from "react";

import "./App.css";

import { InputText } from "primereact/inputtext";
import ChartBar from "./components/CharBar";

const reducer = (state, { type, data }) => {
  switch (type) {
    case "update":
      let { rowIndex, rowData } = data ?? {};
      state.results[rowIndex] = rowData;
      return { ...state };
    case "init":
      return { ...state, results: data };
    default:
      throw new Error();
  }
};

const defaultData = {
  results: [
    { id: 1, name: "Jone", address: "New York" },
    { id: 2, name: "Maya", address: "New York" },
  ],
};

const init = (initialState) => initialState;

const textEditor = (options) => {
  return (
    <InputText
      type="text"
      value={options.value}
      onChange={(e) => options.editorCallback(e.target.value)}
      onBlur={(e) => {
        const { props, rowData, rowIndex } = options;
        const { onRowEditComplete, value: dataTable } = props;
        dataTable[rowIndex]["loading"] = true;
        onRowEditComplete({ rowData, rowIndex });
      }}
      onKeyDown={(e) => e.stopPropagation()}
    />
  );
};

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
            refChart.current.update()
          }
        }}
      >
        reset
      </button>
      <ChartBar
        ref={refChart}
      />
    </div>
  );
};

export default App;
