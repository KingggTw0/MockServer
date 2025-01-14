import { screen } from '@testing-library/react';
import ReactDOM from 'react-dom/client';
import Home from '../components/Home';
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

test('renders Home component', () => {
  // eslint-disable-next-line
  act(()=>{
    ReactDOM.createRoot(container).render(<Home />)
  });
  const text = screen.getByText(/Home/i);
  expect(text).toBeInTheDocument();
});
