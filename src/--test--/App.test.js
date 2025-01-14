import { screen } from '@testing-library/react';
import ReactDOM from 'react-dom/client';
import App from '../App';
import { act } from 'react';

let container;

beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
});

afterEach(() => {
  document.body.removeChild(container);
  container = null;
});

test('renders learn react link', () => {
  // eslint-disable-next-line
  act(()=>{
    ReactDOM.createRoot(container).render(<App />)
  });
  const linkElement = screen.getByText(/learn react/i);
  expect(linkElement).toBeInTheDocument();
});
