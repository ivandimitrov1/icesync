import React, { useState, useEffect } from 'react';
import './App.css';
import { fetchWorkflows, runWorkflow } from './SyncAppApi';
import Table from 'react-bootstrap/Table';
import 'bootstrap/dist/css/bootstrap.min.css';
import Modal from 'react-bootstrap/Modal';

function App() {
  const [showModal, setShowModal] = useState(false);
  const [modalText, setModalText] = useState(null);
  const [modalSuccess, setModalSuccess] = useState(false);
  const [data, setData] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const loadData = async () => {
      try {
        const workflows = await fetchWorkflows();
        setData(workflows);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    loadData();
  }, []);

  const handleRunClick = async (workflowId) => {

    try {
      const isSuccessful = await runWorkflow(workflowId);
      if (isSuccessful) {
        setShowModal(true);
        setModalText("Workflow run.");
        setModalSuccess("bg-success");
      } else {
        setShowModal(true);
        setModalText("Workflow failed to run.");
        setModalSuccess("bg-danger");
      }
    } catch (error) {
      alert(`Failed to run workflow ${workflowId}: ${error.message}`);
    }

  };
  
  const handleCloseModal = () => setShowModal(false);

  if (loading) {
    return <div className="loading">Loading data...</div>;
  }

  if (error) {
    return <div className="error">Error: {error}</div>;
  }

  return (
    <div className="App">
      <h1>Worfklows</h1>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Worfklow Id</th>
            <th>Worfklow Name</th>
            <th>Is Active</th>
            <th>Multi Exec Behavior</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {data.map(workflow => (
            <tr key={workflow.id}>
              <td>{workflow.workflowId}</td>
              <td>{workflow.workflowName}</td>
              <td>{workflow.isActive.toString()}</td>
              <td>{workflow.multiExecBehavior}</td>
              <td><button onClick={() => handleRunClick(workflow.workflowId)} className="run-button">Run</button></td>
            </tr>
          ))}
        </tbody>
      </Table>

      <Modal show={showModal} onHide={handleCloseModal}>
      <Modal.Header closeButton className={modalSuccess}>
        <Modal.Title></Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <p>{modalText}</p>
      </Modal.Body>
    </Modal>
    </div>
  );
}

export default App;