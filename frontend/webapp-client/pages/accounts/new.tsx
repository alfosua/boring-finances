import { NextPage } from 'next';
import NewAccountForm from '../../components/accounts/NewAccountForm';
import AppLayout from '../../components/layouts/AppLayout';
import styles from './new.module.css';

const NewAccountPage: NextPage = () => {
  return (
    <AppLayout>
      <NewAccountForm className={styles.newAccountForm}/>
    </AppLayout>
  );
};

export default NewAccountPage;
